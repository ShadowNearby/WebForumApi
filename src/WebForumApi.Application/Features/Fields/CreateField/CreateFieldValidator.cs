using FluentValidation;
using System.Linq;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Fields.CreateField;

public class CreateFieldValidator : AbstractValidator<CreateFieldRequest>
{
    public CreateFieldValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // cannot exist same tag
        RuleFor(x => x.Content).NotEmpty().MaximumLength(64).Must(x => context.Fields.Any(x => x.Content.Equals(x.Content)));
    }
}