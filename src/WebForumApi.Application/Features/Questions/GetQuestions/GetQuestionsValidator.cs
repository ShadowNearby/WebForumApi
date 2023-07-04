using FluentValidation;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public class GetQuestionsValidator : AbstractValidator<GetQuestionsRequest>
{
    public GetQuestionsValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // tab in ["newest", "heat", "unanswered"]
        RuleFor(x => x.Tab).Must(x => x is null or "" or "newest" or "heat" or "unanswered");
    }
}