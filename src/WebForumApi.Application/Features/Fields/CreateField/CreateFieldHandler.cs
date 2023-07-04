using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Fields.CreateField;

public class CreateFieldHandler : IRequestHandler<CreateFieldRequest, Result>
{
    private readonly IContext _context;

    public CreateFieldHandler(IContext context)
    {
        _context = context;
    }
    public async Task<Result> Handle(CreateFieldRequest request, CancellationToken cancellationToken)
    {
        Field field = new()
        {
            Content = request.Content
        };
        await _context.Fields.AddAsync(field, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}