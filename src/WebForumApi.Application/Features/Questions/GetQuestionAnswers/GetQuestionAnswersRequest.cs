using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionAnswers;

public record GetQuestionAnswersRequest(Guid QuestionId) : PaginatedRequest,
    IRequest<PaginatedList<AnswerDto>>;