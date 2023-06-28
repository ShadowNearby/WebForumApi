namespace WebForumApi.Application.Features.Questions.Dto;

public record UserActionDto
{
    public bool UserLike { get; set; }
    public bool UserDislike { get; set; }
    public bool UserStar { get; set; }
}