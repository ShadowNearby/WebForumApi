using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Tags.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Tags.SearchTag;

public class SearchTagHandler : IRequestHandler<SearchTagRequest, PaginatedList<TagDto>>
{
    private readonly IContext _context;

    public SearchTagHandler(IContext context)
    {
        _context = context;
    }

    public Task<PaginatedList<TagDto>> Handle(SearchTagRequest request, CancellationToken cancellationToken)
    {
        if (request.Keyword.IsNullOrEmpty())
        {
            DbSet<Tag>? tags = _context.Tags;
            return tags.Select(t => new TagDto
                {
                    Id = t.Id, Content = t.Content, Description = t.Description, QuestionCount = _context.QuestionTags.Count(x => x.TagId == t.Id)
                }).OrderByDescending(x => x.QuestionCount)
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
        else
        {
            IQueryable<Tag> tags = _context.Tags.Where(x => x.Content.Contains(request.Keyword ?? ""));
            return tags.Select(t => new TagDto
                {
                    Id = t.Id, Content = t.Content, Description = t.Description, QuestionCount = _context.QuestionTags.Count(x => x.TagId == t.Id)
                }).OrderByDescending(x => x.QuestionCount)
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}