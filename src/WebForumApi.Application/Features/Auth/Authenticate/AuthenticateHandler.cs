using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using BC = BCrypt.Net.BCrypt;


namespace WebForumApi.Application.Features.Auth.Authenticate;

public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, Result<Jwt>>
{
    private readonly TokenConfiguration _appSettings;
    private readonly IContext _context;

    public AuthenticateHandler(IOptions<TokenConfiguration> appSettings, IContext context)
    {
        _context = context;
        _appSettings = appSettings.Value;
    }

    public async Task<Result<Jwt>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(
            x => string.Equals(x.Username, request.Username, StringComparison.Ordinal), cancellationToken);
        if (user == null || !BC.Verify(request.Password, user.Password))
        {
            return Result.Invalid(new List<ValidationError>
            {
                new()
                {
                    Identifier = $"{nameof(request.Password)}|{nameof(request.Username)}",
                    ErrorMessage = "Username or password is incorrect"
                }
            });
        }

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        ClaimsIdentity claims = new(new Claim[]
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        });

        DateTime expDate = DateTime.UtcNow.AddHours(4);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = claims,
            Expires = expDate,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _appSettings.Audience,
            Issuer = _appSettings.Issuer
        };
        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

        return new Jwt { AccessToken = tokenHandler.WriteToken(token), Expire = expDate };
    }
}