using FluentValidation;

namespace WebForumApi.Application.Features.Questions.GetQuestionByTag;

public class GetQuestionByTagValidator : AbstractValidator<GetQuestionsByTagRequest>
{
    public GetQuestionByTagValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // tab in ["newest", "heat"]
        RuleFor(x => x.Tab).Must(x => x is null or "newest" or "heat");
    }
}