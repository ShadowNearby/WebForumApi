using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Auth.Forget;

public class ForgetValidator : AbstractValidator<ForgetRequest>
{
    public ForgetValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Username).NotEmpty().MaximumLength(254);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5).MaximumLength(255);

        RuleFor(x => x.Email).NotEmpty().MaximumLength(254).EmailAddress();
    }
}
