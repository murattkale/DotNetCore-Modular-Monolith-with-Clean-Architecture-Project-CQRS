using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.LangDisplays.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.LangDisplays.Queries;

public class
    GetAllLangDisplayQueryHandler : IRequestHandler<GetAllLangDisplayQuery,
    ResponseModel<PagedResult<LangDisplayDto>>>
{
    private readonly ILangDisplayRepository _LangDisplayRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllLangDisplayQueryHandler(ILangDisplayRepository LangDisplayRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _LangDisplayRepository = LangDisplayRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<LangDisplayDto>>> Handle(GetAllLangDisplayQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(LangDisplay), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<LangDisplay, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _LangDisplayRepository.GetPagedAsync(request.PagedRequest);
            var LangDisplayDtoList = _mapper.Map<List<LangDisplayDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<LangDisplayDto>(LangDisplayDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<LangDisplayDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<LangDisplayDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}