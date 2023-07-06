using Mapster;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Tag.SearchTag;

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
            IOrderedQueryable<Domain.Entities.Tag> tags = _context.Tags.OrderByDescending(x => x.QuestionTags.Count);
            return tags.ProjectToType<TagDto>()
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
        else
        {
            IOrderedQueryable<Domain.Entities.Tag> tags = _context.Tags.Where(x => x.Content.Contains(request.Keyword))
                .OrderByDescending(x => x.QuestionTags.Count);
            return tags.ProjectToType<TagDto>()
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}