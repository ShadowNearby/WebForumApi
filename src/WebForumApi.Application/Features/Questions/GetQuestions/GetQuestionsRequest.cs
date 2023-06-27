using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Application.Features.Users.GetUsers;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public record GetQuestionsRequest : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>
{
    public string Tab { get; init; } = null!;
    public string? KeyWord { get; init; }
}