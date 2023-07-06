using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Tag.Dto;

namespace WebForumApi.Application.Features.Questions.Dto;

public record QuestionCardDto
{
    public string Id { get; init; } = null!;
    public string Title { get; set; } = null!;
    public long VoteNumber { get; set; } = 0;
    public DateTime CreateTime { get; init; }
    public long AnswerNumber { get; set; } = 0;
    public string? AcceptedAnswerId { get; init; }
    public List<TagDto> Tags { get; set; } = new();
}