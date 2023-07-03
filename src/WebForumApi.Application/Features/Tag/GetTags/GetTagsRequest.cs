using MediatR;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Tag.GetTags;

public record GetTagsRequest : PaginatedRequest, IRequest<PaginatedList<TagDto>>
{
    public string? Keyword { get; set; }
}