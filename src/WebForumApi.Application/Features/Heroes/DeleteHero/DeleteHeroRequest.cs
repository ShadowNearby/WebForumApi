using Ardalis.Result;
using MediatR;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Heroes.DeleteHero;

public record DeleteHeroRequest(HeroId Id) : IRequest<Result>;