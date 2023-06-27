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
            (_, tags) => tags.TrueForAll(t => context.Tags.Any(tg => tg.Id == t.Id))
        );
    }
}