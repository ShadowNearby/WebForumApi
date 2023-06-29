using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersByUserId;

public record GetAnswersByUserIdRequest(Guid Id) : PaginatedRequest, IRequest<PaginatedList<AnswerCardDto>>;