using FluentValidation;

namespace WebForumApi.Application.Features.Users.DeleteUser;

public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}