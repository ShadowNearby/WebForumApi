using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public record QuestionStarRequest(string Id) : IRequest<Result>;