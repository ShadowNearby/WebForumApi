using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsByUserId;

public record GetQuestionsByUserIdRequest(Guid Id) : IRequest<Result<List<QuestionCardDto>>>;