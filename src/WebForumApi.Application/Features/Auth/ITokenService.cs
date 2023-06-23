using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Auth;

public interface ITokenService
{
    Jwt GenerateJwt(string username, string role, UserId id);
}