using Ardalis.Result;
using MediatR;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Heroes.GetHeroById;

public record GetHeroByIdRequest(HeroId Id) : IRequest<Result<GetHeroResponse>>;
