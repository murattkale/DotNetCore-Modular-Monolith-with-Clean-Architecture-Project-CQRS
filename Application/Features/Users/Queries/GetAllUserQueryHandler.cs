using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Users.Queries;

public class
    GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery,
    ResponseModel<PagedResult<UserDto>>>
{
    private readonly IUserRepository _UserRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllUserQueryHandler(IUserRepository UserRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _UserRepository = UserRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<UserDto>>> Handle(GetAllUserQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(User), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<User, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _UserRepository.GetPagedAsync(request.PagedRequest);
            var UserDtoList = _mapper.Map<List<UserDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<UserDto>(UserDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<UserDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<UserDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}