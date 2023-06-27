using Ardalis.Result;
using FluentValidation;
using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestions;

namespace WebForumApi.Application.Features.Questions.GetQuestionByTag;

public record GetQuestionsByTagRequest : PaginatedRequest, IRequest<PaginatedList<QuestionCardDto>>
{
    public string TagName { get; set; } = null!;
    public string Tab { get; set; } = null!;
    public string? KeyWord { get; set; }
}

public class GetQuestionByTagValidator : AbstractValidator<GetQuestionsRequest>
{
    public GetQuestionByTagValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // tab in ["newest", "heat"]
        RuleFor(x => x.Tab).NotEmpty().Must(x => x.Equals("newest") || x.Equals("heat"));
    }
}