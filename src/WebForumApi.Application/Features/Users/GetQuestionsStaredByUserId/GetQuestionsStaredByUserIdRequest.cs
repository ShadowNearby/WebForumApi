using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsStaredByUserId;

public record GetQuestionsStaredByUserIdRequest(Guid Id) : IRequest<Result<List<QuestionCardDto>>>;