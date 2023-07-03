using Ardalis.Result;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Tag.ModifyTag;

public class ModifyTagHandler : IRequestHandler<ModifyTagRequest, Result>
{
    private readonly IContext _context;

    public ModifyTagHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(ModifyTagRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Tag tag = _context.Tags.FirstOrDefault(t => t.Id == request.TagId)!;
        tag.Description = request.Description;
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}