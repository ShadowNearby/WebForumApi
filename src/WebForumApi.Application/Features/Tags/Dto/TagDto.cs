namespace WebForumApi.Application.Features.Tags.Dto;

public record TagDto
{
    public long Id { get; init; }
    public string Content { get; init; } = null!;
    public string? Description { get; init; }

    public int? QuestionCount { get; init; } = null;
}