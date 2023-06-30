using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersLikedByUserId;

public record GetAnswersLikedByUserIdRequest : PaginatedRequest, IRequest<PaginatedList<AnswerCardDto>>
{
    public Guid Id { get; init; }
}