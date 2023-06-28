using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Common;
using ISession=WebForumApi.Domain.Auth.Interfaces.ISession;

namespace WebForumApi.Application.Auth;

public class Session : ISession
{
    public UserId UserId { get; private init; }
    public string? Username { get; private init; }
    public string? Avatar { get; private init; }
    public DateTime Now => DateTime.Now;

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        ClaimsPrincipal? userClaimsPrincipal = httpContextAccessor.HttpContext?.User;

        Claim? idIdentifier = userClaimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier);
        Claim? usernameIdentifier = userClaimsPrincipal?.FindFirst(ClaimTypes.Name);
        Claim? avatarIdentifier = userClaimsPrincipal?.FindFirst(ClaimTypes.Uri);
        if (idIdentifier != null)
        {
            UserId = new Guid(idIdentifier.Value);
        }

        if (usernameIdentifier != null)
        {
            Username = usernameIdentifier.Value;
        }

        if (avatarIdentifier != null)
        {
            Avatar = avatarIdentifier.Value;
        }
    }
}