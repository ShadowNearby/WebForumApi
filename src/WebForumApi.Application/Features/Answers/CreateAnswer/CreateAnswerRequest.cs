using Ardalis.Result;
using MediatR;
using System;
using System.Reflection;

namespace WebForumApi.Application.Features.Answers.CreateAnswer;

public class CreateAnswerRequest : IRequest<Result>
{
    public string Content { get; set; } = null!;
    public string QuestionId { get; set; } = null!;
}