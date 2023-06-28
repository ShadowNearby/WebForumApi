using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Auth;

public interface ITokenService
{
    JwtDto GenerateJwt(string username, string role, UserId id, string avatar);
}