using Ardalis.Result;
using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsByUserId;

public record GetQuestionsByUserIdRequest : PaginatedRequest, IRequest<Result<PaginatedList<QuestionCardDto>>>
{
    public Guid Id { get; init; }
}