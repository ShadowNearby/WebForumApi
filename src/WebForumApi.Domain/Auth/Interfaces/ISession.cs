using System;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }

    public DateTime Now { get; }
}
