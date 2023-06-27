using System.Collections.Generic;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.Dto;

public record QuestionCardDto
{
    public string Id { get; init; } = null!;
    public string Title { get; set; } = null!;
    public long VoteNumber { get; set; } = 0;
    public long AnswerNumber { get; set; } = 0;
    public List<Tag> Tags { get; set; } = new();
}