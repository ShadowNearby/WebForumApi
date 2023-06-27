using Ardalis.Result;
using MediatR;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.DeleteUser;

public record DeleteUserRequest(UserId Id) : IRequest<Result>;