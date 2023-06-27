using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Auth.TokenService;

public class TokenService : ITokenService
{
    private readonly TokenConfiguration _appSettings;
    private readonly JwtSecurityTokenHandler _handler = new();

    public TokenService(IOptions<TokenConfiguration> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public JwtDto GenerateJwt(string username, string role, UserId id)
    {
        byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        ClaimsIdentity claims =
            new(
                new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, id.ToString()), new(ClaimTypes.Name, username), new(ClaimTypes.Role, role)
                }
            );

        DateTime expDate = DateTime.Now.AddHours(4);

        SecurityTokenDescriptor tokenDescriptor =
            new()
            {
                Subject = claims,
                Expires = expDate,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Audience = _appSettings.Audience,
                Issuer = _appSettings.Issuer
            };
        SecurityToken? accessToken = _handler.CreateToken(tokenDescriptor);
        byte[] randomNumber = new byte[32];
        RandomNumberGenerator.Create().GetBytes(randomNumber);
        string refreshToken = Convert.ToBase64String(randomNumber);
        return new JwtDto
        {
            AccessToken = _handler.WriteToken(accessToken),
            RefreshToken = refreshToken,
            Expire = expDate
        };
    }
}