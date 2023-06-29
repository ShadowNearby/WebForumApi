using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersLikedByUserId;

public record GetAnswersLikedByUserIdRequest(Guid Id) : IRequest<Result<List<AnswerCardDto>>>;