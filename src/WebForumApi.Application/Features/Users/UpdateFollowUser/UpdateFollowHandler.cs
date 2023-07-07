using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Users.UpdateFollowUser;

public class UpdateFollowHandler : IRequestHandler<UpdateFollowRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public UpdateFollowHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<Result> Handle(UpdateFollowRequest request, CancellationToken cancellationToken)
    {
        UserId userId = _session.UserId;
        Guid followId = request.Id;
        User? user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        User? followUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == followId, cancellationToken);
        if (user == null || followUser == null)
        {
            return Result.Invalid(new List<ValidationError>
            {
                new()
                {
                    Identifier = $"{userId} or {followId}", ErrorMessage = "userId or followId error"
                }
            });
        }

        UserFollow? follow = await _context.UserFollows.FirstOrDefaultAsync(f => f.UserId == userId && followId == f.UserIdFollowing, cancellationToken);
        if (follow != null)
        {
            if (request.IsFollow == false)
            {
                _context.UserFollows.Remove(follow);
                user.FollowingCount -= 1;
                followUser.FollowedCount -= 1;
            }
        }
        else
        {
            if (request.IsFollow)
            {
                await _context.UserFollows.AddAsync(new UserFollow
                    {
                        UserId = userId, UserIdFollowing = followId
                    },
                    cancellationToken);
                user.FollowingCount += 1;
                followUser.FollowedCount += 1;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}