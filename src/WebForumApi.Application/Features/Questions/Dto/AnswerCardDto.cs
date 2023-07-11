using System;

namespace WebForumApi.Application.Features.Questions.Dto;

public record AnswerCardDto
{
    public string QuestionId { get; init; } = null!;
    public string Id { get; init; } = null!;
    public string Content { get; set; } = null!;
    public long VoteNumber { get; init; } = 0;

    public DateTime CreateTime { get; init; }
}