using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Tag.Dto;

namespace WebForumApi.Application.Features.Tag.GetTags;

public class GetTagsHandler : IRequestHandler<GetTagsRequest, Result<PaginatedList<TagDto>>>
{
    private readonly IContext _context;

    public GetTagsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<TagDto>>> Handle(GetTagsRequest request, CancellationToken cancellationToken)
    {
        if (request.Keyword.IsNullOrEmpty())
        {
            IQueryable<Domain.Entities.Tag> tags = _context.Tags;
            return await tags.Select(t => new TagDto
                {
                    Id = t.Id, Content = t.Content, Description = t.Description, QuestionCount = _context.QuestionTags.Count(x => x.TagId == t.Id)
                }).OrderByDescending(x => x.QuestionCount)
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
        else
        {
            IQueryable<Domain.Entities.Tag> tags = _context.Tags.Where(
                x => EF.Functions.Like(x.Content, $"%{request.Keyword}%")
            );
            return await tags.Select(t => new TagDto
            {
                Id = t.Id, Content = t.Content, Description = t.Description, QuestionCount = _context.QuestionTags.Count(x => x.TagId == t.Id)
            }).OrderByDescending(x => x.QuestionCount).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}