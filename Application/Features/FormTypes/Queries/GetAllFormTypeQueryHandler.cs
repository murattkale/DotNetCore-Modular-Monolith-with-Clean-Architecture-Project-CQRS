using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.FormTypes.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.FormTypes.Queries;

public class GetAllFormTypeQueryHandler : IRequestHandler<GetAllFormTypeQuery,
    ResponseModel<PagedResult<FormTypeDto>>>
{
    private readonly IFormTypeRepository _FormTypeRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllFormTypeQueryHandler(IFormTypeRepository FormTypeRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _FormTypeRepository = FormTypeRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<FormTypeDto>>> Handle(GetAllFormTypeQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(FormType), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<FormType, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _FormTypeRepository.GetPagedAsync(request.PagedRequest);
            var FormTypeDtoList = _mapper.Map<List<FormTypeDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<FormTypeDto>(FormTypeDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<FormTypeDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<FormTypeDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}