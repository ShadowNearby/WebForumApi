﻿using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Tags.CreateTag;

public class CreateTagHandler : IRequestHandler<CreateTagRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public CreateTagHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(CreateTagRequest request, CancellationToken cancellationToken)
    {
        Tag tag = new()
        {
            Content = request.Content, Description = request.Description
        };
        await _context.Tags.AddAsync(tag, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}