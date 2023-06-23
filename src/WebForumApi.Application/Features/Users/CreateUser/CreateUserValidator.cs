using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(254)
            .MustAsync(async (username, ct) =>
                !await context.Users.AnyAsync(
                    y => y.Username.Equals(username), ct))
            .WithMessage("A user with this username already exists.");
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(255);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress()
            .MustAsync(async (email, ct) =>
                !await context.Users.AnyAsync(y => y.Email.Equals(email), ct))
            .WithMessage("A user with this email already exists.");
    }
}