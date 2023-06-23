using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using WebForumApi.Domain.Entities.Common;
using ISession = WebForumApi.Domain.Auth.Interfaces.ISession;

namespace WebForumApi.Application.Auth;

public class Session : ISession
{
    public UserId UserId { get; private init; }

    public DateTime Now => DateTime.Now;

    public Session(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;

        var nameIdentifier = user?.FindFirst(ClaimTypes.NameIdentifier);

        if (nameIdentifier != null)
        {
            UserId = new Guid(nameIdentifier.Value);
        }
    }
}
