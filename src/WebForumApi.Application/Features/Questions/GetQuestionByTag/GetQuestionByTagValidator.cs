using FluentValidation;
using WebForumApi.Application.Features.Questions.GetQuestions;

namespace WebForumApi.Application.Features.Questions.GetQuestionByTag;

public class GetQuestionByTagValidator : AbstractValidator<GetQuestionsRequest>
{
    public GetQuestionByTagValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // tab in ["newest", "heat"]
        RuleFor(x => x.Tab).Must(x => x is null || x.Equals("newest") || x.Equals("heat"));
    }
}