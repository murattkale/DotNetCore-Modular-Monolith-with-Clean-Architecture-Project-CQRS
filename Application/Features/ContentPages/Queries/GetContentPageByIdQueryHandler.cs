using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ContentPages.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Queries;

public class
    GetContentPageByIdQueryHandler : IRequestHandler<GetContentPageByIdQuery, ResponseModel<ContentPageDto>>
{
    private readonly IContentPageRepository _contentPageRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetContentPageByIdQueryHandler(IContentPageRepository contentPageRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _contentPageRepository = contentPageRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<ContentPageDto>> Handle(GetContentPageByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var contentPage = await _contentPageRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (contentPage == null) return ResponseModel<ContentPageDto>.Failure("Content page not found.");

            var contentPageDto = _mapper.Map<ContentPageDto>(contentPage);
            return ResponseModel<ContentPageDto>.Success(contentPageDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<ContentPageDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}