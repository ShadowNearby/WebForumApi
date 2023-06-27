using FluentValidation;
using System.Linq;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionRequest>
{
    public CreateQuestionValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Tags).Must(
            (tags, ct) => tags.Tags.TrueForAll(t => context.Tags.Any(tg => tg.Id == t.Id))
        );
    }
}