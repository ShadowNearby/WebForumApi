namespace WebForumApi.Application.Features.Questions.Dto;

public record TagDto
{
    public string Id { get; init; } = null!;
    public string Content { get; init; } = null!;
}