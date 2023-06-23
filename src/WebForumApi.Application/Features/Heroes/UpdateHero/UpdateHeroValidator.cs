using FluentValidation;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Heroes.UpdateHero;

public class UpdateHeroValidator : AbstractValidator<UpdateHeroRequest>
{
    public UpdateHeroValidator()
    {

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.HeroType)
            .IsInEnum();

        RuleFor(x => x.Age)
            .GreaterThan(0);

        RuleFor(x => x.Nickname)
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.Team)
            .MaximumLength(StringSizes.Max);
    }
}