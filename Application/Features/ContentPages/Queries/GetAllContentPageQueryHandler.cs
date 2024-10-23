using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ContentPages.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class
    GetAllContentPageQueryHandler : IRequestHandler<GetAllContentPageQuery,
    ResponseModel<PagedResult<ContentPageDto>>>
{
    private readonly IContentPageRepository _contentPageRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllContentPageQueryHandler(IContentPageRepository contentPageRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _contentPageRepository = contentPageRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<ContentPageDto>>> Handle(GetAllContentPageQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // request.PagedRequest.Predicate = cp =>
            //     (!request.ParentId.HasValue || cp.ParentId == request.ParentId) &&
            //     (!request.ContentTypes.HasValue || cp.ContentTypes == request.ContentTypes);

            // var parameter1 = Expression.Parameter(typeof(ContentPage), "x");
            // var property1 = Expression.Property(parameter1, request.PagedRequest.OrderByColumn);

           
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(ContentPage), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<ContentPage, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _contentPageRepository.GetPagedAsync(request.PagedRequest);
            var contentPageDtoList = _mapper.Map<List<ContentPageDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<ContentPageDto>(contentPageDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<ContentPageDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<ContentPageDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}