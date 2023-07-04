using Ardalis.Result;
using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersStaredByUserId;

public record GetAnswersStaredByUserIdRequest : PaginatedRequest, IRequest<Result<PaginatedList<AnswerCardDto>>>
{
    public Guid Id { get; init; }
}