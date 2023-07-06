using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Application.Features.Questions.Dto;

public record QuestionDto
{
    public string Id { get; init; } = null!;
    public UserCardDto UserCard { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string Content { get; init; } = null!;
    public long StarCount { get; set; }
    public long LikeCount { get; set; }
    public long DislikeCount { get; set; }
    public UserActionDto UserAction { get; set; } = null!;
    public DateTime CreateTime { get; init; }

    public DateTime? LastEdit { get; init; }
    public List<AnswerDto> Answers { get; init; } = null!;
    public List<TagDto> Tags { get; init; } = null!;
}