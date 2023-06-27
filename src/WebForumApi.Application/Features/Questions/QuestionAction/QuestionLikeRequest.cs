using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionLikeRequest : IRequest<Result>
{
    public string Id { get; set; } = null!;
}

public class QuestionDislikeRequest : IRequest<Result>
{
    public string Id { get; set; } = null!;
}

public class QuestionStarRequest : IRequest<Result>
{
    public string Id { get; set; } = null!;
}