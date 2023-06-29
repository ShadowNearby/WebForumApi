using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Answers.AnswerAction;

public record AnswerLikeRequest(string Id) : IRequest<Result>;

public record AnswerDislikeRequest(string Id) : IRequest<Result>;

public record AnswerStarRequest(string Id) : IRequest<Result>;