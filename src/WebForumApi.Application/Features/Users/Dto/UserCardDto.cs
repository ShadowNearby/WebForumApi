namespace WebForumApi.Application.Features.Users.Dto;

public record UserCardDto
{
    public string Id { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public string? Avatar { get; init; }
}