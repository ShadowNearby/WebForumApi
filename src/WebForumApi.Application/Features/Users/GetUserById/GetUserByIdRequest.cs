using Ardalis.Result;
using WebForumApi.Domain.Entities.Common;
using MediatR;

namespace WebForumApi.Application.Features.Users.GetUserById;

public record GetUserByIdRequest(UserId Id) : IRequest<Result<GetUserResponse>>;