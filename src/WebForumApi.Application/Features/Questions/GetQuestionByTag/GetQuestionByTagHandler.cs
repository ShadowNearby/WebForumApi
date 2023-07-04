using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.GetQuestionByTag;

public class GetQuestionByTagHandler : IRequestHandler<GetQuestionsByTagRequest, Result<PaginatedList<QuestionCardDto>>>
{
    private readonly IContext _context;

    public GetQuestionByTagHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<QuestionCardDto>>> Handle(GetQuestionsByTagRequest request,
        CancellationToken cancellationToken)
    {
        switch (request.Tab)
        {
            case "newest":
                // get questions by tag and keyword and order by create time
                IQueryable<Question> newestQuestions =
                    _context.Questions.Where(q => q.QuestionTags.Any(qt => qt.Tag.Content == request.TagName))
                        .OrderByDescending(x => x.CreateTime)
                        .WhereIf(!string.IsNullOrEmpty(request.KeyWord),
                            x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%"));
                return await newestQuestions.ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
            default:
                // get questions by tag and keyword and order by answer number
                IQueryable<Question> heatQuestions =
                    _context.Questions.Where(q => q.QuestionTags.Any(qt => qt.Tag.Content == request.TagName))
                        .OrderBy(x => x.Answers.Count)
                        .WhereIf(!string.IsNullOrEmpty(request.KeyWord),
                            x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%"));
                return await heatQuestions.ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}