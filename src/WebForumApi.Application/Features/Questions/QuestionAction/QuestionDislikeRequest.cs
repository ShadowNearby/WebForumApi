using Ardalis.Result;
using MediatR;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public record QuestionDislikeRequest(string Id) : IRequest<Result>;