using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public record QuestionLikeRequest(string Id) : IRequest<Result>;