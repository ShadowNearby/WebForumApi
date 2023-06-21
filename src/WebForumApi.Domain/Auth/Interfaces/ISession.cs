using WebForumApi.Domain.Entities.Common;
using System;

namespace WebForumApi.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }

    public DateTime Now { get; }
}