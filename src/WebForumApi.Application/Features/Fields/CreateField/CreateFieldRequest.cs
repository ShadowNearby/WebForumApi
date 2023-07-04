using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Fields.CreateField;

public record CreateFieldRequest : IRequest<Result>
{
    public string Content { get; init; } = null!;
}