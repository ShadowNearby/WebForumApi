using Ardalis.Result;
using WebForumApi.Domain.Entities.Common;
using MediatR;

namespace WebForumApi.Application.Features.Users.DeleteUser;

public record DeleteUserRequest(UserId Id) : IRequest<Result>;