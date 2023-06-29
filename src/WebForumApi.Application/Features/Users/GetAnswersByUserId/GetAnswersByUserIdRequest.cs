using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersByUserId;

public record GetAnswersByUserIdRequest(Guid Id) : IRequest<Result<List<AnswerCardDto>>>;