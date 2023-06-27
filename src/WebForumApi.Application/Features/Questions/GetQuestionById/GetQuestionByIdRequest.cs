using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public record GetQuestionByIdRequest() : IRequest<Result<QuestionDto>>
{
    public string Id { get; init; } = null!;
}