using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionLikeRequest : IRequest<Result>
{
    public string QuestionId { get; set; } = null!;
}

public class QuestionDislikeRequest : IRequest<Result>
{
    public string QuestionId { get; set; } = null!;
}

public class QuestionStarRequest : IRequest<Result>
{
    public string QuestionId { get; set; } = null!;
}