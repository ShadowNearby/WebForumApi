using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public record GetQuestionsRequest : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>
{
    public string Tab { get; init; } = "";

    // tab in ["newest", "heat", "unanswered"]
    public string? KeyWord { get; init; }
}