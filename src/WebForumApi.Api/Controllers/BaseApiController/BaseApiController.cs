using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    protected readonly IMediator Mediator;
    protected readonly ISession Session;

    public BaseApiController(IMediator mediator, ISession session)
    {
        Mediator = mediator;
        Session = session;
    }
}