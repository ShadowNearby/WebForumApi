using FluentValidation;

namespace WebForumApi.Application.Features.Auth.Refresh;

public class RefreshValidator : AbstractValidator<RefreshRequest>
{
    public RefreshValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}