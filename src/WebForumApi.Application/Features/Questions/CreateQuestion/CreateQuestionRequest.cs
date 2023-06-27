using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public record CreateQuestionRequest : IRequest<Result>
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public List<TagDto> Tags { get; set; } = new();
}