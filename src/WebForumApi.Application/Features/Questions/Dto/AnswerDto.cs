using System;
using WebForumApi.Application.Features.Users.Dto;

namespace WebForumApi.Application.Features.Questions.Dto;

public record AnswerDto
{
    public string Id { get; init; } = null!;
    public UserCardDto UserCard { get; init; } = null!;
    public string Content { get; init; } = null!;
    public DateTime CreateTime { get; init; }
    public long StarCount { get; init; }
    public long LikeCount { get; init; }
    public long DislikeCount { get; init; }
    public UserActionDto UserAction { get; set; } = null!;
}