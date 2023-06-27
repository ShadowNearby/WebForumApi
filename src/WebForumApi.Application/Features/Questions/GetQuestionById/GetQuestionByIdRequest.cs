using Ardalis.Result;
using MediatR;
using System;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public record GetQuestionByIdRequest(Guid Id, UserId UserId) : IRequest<Result<QuestionDto>>;