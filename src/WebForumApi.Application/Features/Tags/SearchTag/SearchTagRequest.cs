using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Tags.Dto;

namespace WebForumApi.Application.Features.Tags.SearchTag;

public record SearchTagRequest : PaginatedRequest, IRequest<PaginatedList<TagDto>>
{
    public string? Keyword { get; set; }
}