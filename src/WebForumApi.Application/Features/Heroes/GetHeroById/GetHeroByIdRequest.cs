using Ardalis.Result;
using WebForumApi.Domain.Entities.Common;
using MediatR;

namespace WebForumApi.Application.Features.Heroes.GetHeroById;

public record GetHeroByIdRequest(HeroId Id) : IRequest<Result<GetHeroResponse>>;