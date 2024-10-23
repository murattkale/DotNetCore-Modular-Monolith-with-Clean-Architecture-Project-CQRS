using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Documents.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Queries;

public class
    GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery,
    ResponseModel<PagedResult<DocumentsDto>>>
{
    private readonly IDocumentsRepository _DocumentsRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetAllDocumentsQueryHandler(IDocumentsRepository DocumentsRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _DocumentsRepository = DocumentsRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<PagedResult<DocumentsDto>>> Handle(GetAllDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.PagedRequest.OrderByColumn))
            {
                var parameter = Expression.Parameter(typeof(dotnetcoreproject.Domain.Entities.Documents), "x");
                var property = Expression.Property(parameter, request.PagedRequest.OrderByColumn);
                var orderByExpression =
                    Expression.Lambda<Func<dotnetcoreproject.Domain.Entities.Documents, object>>(Expression.Convert(property, typeof(object)),
                        parameter);
                request.PagedRequest.OrderBy = orderByExpression;
            }

            var pagedResult = await _DocumentsRepository.GetPagedAsync(request.PagedRequest);
            var DocumentsDtoList = _mapper.Map<List<DocumentsDto>>(pagedResult.Items);
            var pagedDtoResult = new PagedResult<DocumentsDto>(DocumentsDtoList, pagedResult.TotalCount,
                request.PagedRequest.PageNumber, request.PagedRequest.PageSize);

            return ResponseModel<PagedResult<DocumentsDto>>.Success(pagedDtoResult,
                "Content pages retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving all content pages.");
            return ResponseModel<PagedResult<DocumentsDto>>.Failure(
                $"Error retrieving content pages: {ex.Message}");
        }
    }
}