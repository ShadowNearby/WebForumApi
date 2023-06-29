using MediatR;
using System;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsStaredByUserId;

public record GetQuestionsStaredByUserIdRequest(Guid Id) : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>;