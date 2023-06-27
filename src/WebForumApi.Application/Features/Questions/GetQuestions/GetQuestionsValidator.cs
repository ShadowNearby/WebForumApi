using FluentValidation;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public class GetQuestionsValidator : AbstractValidator<GetQuestionsRequest>
{
    public GetQuestionsValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // tab in ["newest", "heat", "unanswered"]
        RuleFor(x => x.Tab).NotEmpty().Must(x => x.Equals("newest") || x.Equals("heat") || x.Equals("unanswered"));
    }
}