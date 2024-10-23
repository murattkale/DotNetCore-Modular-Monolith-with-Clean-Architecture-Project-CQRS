using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Forms.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Queries;

public class
    GetAllFormsQueryHandler : IRequestHandler<GetAllFormsQuery,
    ResponseModel<PagedResult<FormsDto>>>
{
    private readonly IFormsRepository _FormsRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllFormsQueryHandler(IFormsRepository FormsRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _FormsRepository = FormsRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<FormsDto>>> Handle(GetAllFormsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(Domain.Entities.Forms), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<Domain.Entities.Forms, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _FormsRepository.GetPagedAsync(request.PagedRequest);
            var FormsDtoList = _mapper.Map<List<FormsDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<FormsDto>(FormsDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<FormsDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<FormsDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}