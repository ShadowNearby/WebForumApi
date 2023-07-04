using Ardalis.Result;
using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestionByTag;

public record GetQuestionsByTagRequest(string TagName) : PaginatedRequest, IRequest<Result<PaginatedList<QuestionCardDto>>>
{
    public string Tab { get; set; } = null!;
    public string? KeyWord { get; set; }
}

public record GetQuestionsByTagRequestPublic : PaginatedRequest, IRequest<Result<PaginatedList<QuestionCardDto>>>
{
    public string Tab { get; set; } = null!;
    public string? KeyWord { get; set; }
}