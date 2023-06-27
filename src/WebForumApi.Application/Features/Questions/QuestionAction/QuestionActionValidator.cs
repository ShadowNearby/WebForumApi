using FluentValidation;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionLikeValidator : AbstractValidator<QuestionLikeRequest>
{
    public QuestionLikeValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class QuestionDislikeValidator : AbstractValidator<QuestionDislikeRequest>
{
    public QuestionDislikeValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class QuestionStarValidator : AbstractValidator<QuestionStarRequest>
{
    public QuestionStarValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id).NotEmpty();
    }
}