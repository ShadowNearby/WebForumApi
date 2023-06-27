namespace WebForumApi.Application.Features.Questions.Dto;

public record TagDto
{
    public long Id { get; init; }
    public string Content { get; init; } = null!;
    public string? Description { get; init; }
}