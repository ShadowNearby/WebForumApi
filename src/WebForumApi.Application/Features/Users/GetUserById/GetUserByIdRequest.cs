using Ardalis.Result;
using MediatR;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.GetUserById;

public record GetUserByIdRequest(UserId Id) : IRequest<Result<GetUserResponse>>;