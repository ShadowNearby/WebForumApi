using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public record CreateQuestionRequest : IRequest<Result>
{
}