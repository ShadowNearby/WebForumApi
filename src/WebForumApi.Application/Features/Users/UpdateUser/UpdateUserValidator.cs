using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Users.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(5).MaximumLength(254);
        RuleFor(x => x.Username)
            .MaximumLength(254)
            .MustAsync(
                async (username, ct) =>
                    !await context.Users.AnyAsync(y => y.Username.Equals(username), ct)
            )
            .WithMessage("A user with this username already exists.");
        RuleFor(x => x.Email)
            .MaximumLength(254)
            .EmailAddress()
            .MustAsync(
                async (email, ct) => !await context.Users.AnyAsync(y => y.Email.Equals(email), ct)
            )
            .WithMessage("A user with this email already exists.");
        RuleFor(x => x.Avatar).MaximumLength(254);
        RuleFor(x => x.About).MaximumLength(254);
        RuleFor(x => x.Location).MaximumLength(254);
    }
}