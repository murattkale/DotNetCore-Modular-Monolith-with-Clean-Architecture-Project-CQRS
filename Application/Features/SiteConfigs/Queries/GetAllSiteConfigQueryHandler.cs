using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Queries;

public class GetAllSiteConfigQueryHandler : IRequestHandler<GetAllSiteConfigQuery,
    ResponseModel<PagedResult<SiteConfigDto>>>
{
    private readonly ISiteConfigRepository _siteConfigRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllSiteConfigQueryHandler(ISiteConfigRepository siteConfigRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _siteConfigRepository = siteConfigRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<SiteConfigDto>>> Handle(GetAllSiteConfigQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            // request.PagedRequest.Predicate = cp =>
            //     (!request.ParentId.HasValue || cp.ParentId == request.ParentId) &&
            //     (!request.ContentTypes.HasValue || cp.ContentTypes == request.ContentTypes);

            // var parameter1 = Expression.Parameter(typeof(SiteConfig), "x");
            // var property1 = Expression.Property(parameter1, request.PagedRequest.OrderByColumn);

           
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(SiteConfig), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<SiteConfig, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _siteConfigRepository.GetPagedAsync(request.PagedRequest);
            var siteConfigDtoList = _mapper.Map<List<SiteConfigDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<SiteConfigDto>(siteConfigDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<SiteConfigDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<SiteConfigDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}