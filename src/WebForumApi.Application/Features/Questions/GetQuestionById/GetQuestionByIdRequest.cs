using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public record GetQuestionByIdRequest(string Id) : IRequest<Result<QuestionDto>>;