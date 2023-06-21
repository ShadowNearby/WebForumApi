using Ardalis.Result;
using WebForumApi.Domain.Entities.Common;
using MediatR;

namespace WebForumApi.Application.Features.Heroes.DeleteHero;

public record DeleteHeroRequest(HeroId Id) : IRequest<Result>;