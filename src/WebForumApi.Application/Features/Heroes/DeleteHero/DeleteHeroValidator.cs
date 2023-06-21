using FluentValidation;

namespace WebForumApi.Application.Features.Heroes.DeleteHero;

public class DeleteHeroValidator : AbstractValidator<DeleteHeroRequest>
{

    public DeleteHeroValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}