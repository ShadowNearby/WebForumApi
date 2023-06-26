using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Application.Features.Questions.Dto;

public record AnswerDto
{
    public string Id { get; init; } = null!;
    public UserCardDto UserCard { get; init; } = null!;
    public string Content { get; init; } = null!;
    public int StarCount { get; init; }
    public int LikeCount { get; init; }
    public int DislikeCount { get; init; }
    public bool UserStar { get; init; }
    public bool UserLike { get; init; }
    public bool UserDislike { get; init; }
}