using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsByUserId;

public record GetQuestionsByUserIdRequest(Guid Id) : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>;