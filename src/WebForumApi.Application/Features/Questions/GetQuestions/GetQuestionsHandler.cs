using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public class GetQuestionsHandler : IRequestHandler<GetQuestionsRequest, PaginatedList<QuestionCardDto>>
{
    private readonly IContext _context;

    public GetQuestionsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<QuestionCardDto>> Handle(GetQuestionsRequest request,
        CancellationToken cancellationToken)
    {
        // tab in ["newest", "heat", "unanswered"]
        switch (request.Tab)
        {
            case "newest":
                // order by create time
                IQueryable<Question> newestQuestions = _context.Questions
                    .OrderByDescending(x => x.CreateTime)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                return await newestQuestions
                    .ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
            case "heat":
                // order by answer number
                IQueryable<Question> heatQuestions = _context.Questions
                    .OrderByDescending(x => x.Answers.Count)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                return await heatQuestions
                    .ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
            default:
                // select unanswered questions
                var unansweredQuestions = _context.Questions
                    .Where(x => x.Answers.Count == 0)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                return await unansweredQuestions.ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}