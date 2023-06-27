using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Answers.AnswerAction;

public class AnswerLikeRequest : IRequest<Result>
{
    public string AnswerId { get; set; } = null!;
}

public class AnswerDislikeRequest : IRequest<Result>
{
    public string AnswerId { get; set; } = null!;
}

public class AnswerStarRequest : IRequest<Result>
{
    public string AnswerId { get; set; } = null!;
}