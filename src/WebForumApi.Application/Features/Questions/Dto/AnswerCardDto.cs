namespace WebForumApi.Application.Features.Questions.Dto;

public record AnswerCardDto
{
    public string Id { get; init; } = null!;
    public string Content { get; set; } = null!;
    public long VoteNumber { get; set; } = 0;
}