using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Langs.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Queries;

public class
    GetAllLangQueryHandler : IRequestHandler<GetAllLangQuery,
    ResponseModel<PagedResult<LangDto>>>
{
    private readonly ILangRepository _LangRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllLangQueryHandler(ILangRepository LangRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _LangRepository = LangRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<LangDto>>> Handle(GetAllLangQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(Lang), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<Lang, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _LangRepository.GetPagedAsync(request.PagedRequest);
            var LangDtoList = _mapper.Map<List<LangDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<LangDto>(LangDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<LangDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<LangDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}