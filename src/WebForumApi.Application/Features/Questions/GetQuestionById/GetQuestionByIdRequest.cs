using Ardalis.Result;
using MediatR;
using System;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public record GetQuestionByIdRequest(Guid QuestionId) : IRequest<Result<QuestionDto>>;