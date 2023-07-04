using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

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
            IQueryable<Domain.Entities.Tag> tags = _context.Tags.OrderByDescending(x => x.QuestionTags.Count);
            return await tags.ProjectToType<TagDto>()
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
        else
        {
            IQueryable<Domain.Entities.Tag> tags = _context.Tags.Where(
                x => EF.Functions.Like(x.Content, $"%{request.Keyword}%")
            ).OrderByDescending(x => x.QuestionTags.Count);
            return await tags.ProjectToType<TagDto>()
                .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}