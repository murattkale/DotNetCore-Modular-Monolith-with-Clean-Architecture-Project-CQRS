using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Queries;

public class GetSiteConfigByIdQueryHandler : IRequestHandler<GetSiteConfigByIdQuery, ResponseModel<SiteConfigDto>>
{
    private readonly ISiteConfigRepository _SiteConfigRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetSiteConfigByIdQueryHandler(ISiteConfigRepository SiteConfigRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _SiteConfigRepository = SiteConfigRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<SiteConfigDto>> Handle(GetSiteConfigByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var SiteConfig = await _SiteConfigRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (SiteConfig == null) return ResponseModel<SiteConfigDto>.Failure("Content page not found.");

            var SiteConfigDto = _mapper.Map<SiteConfigDto>(SiteConfig);
            return ResponseModel<SiteConfigDto>.Success(SiteConfigDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<SiteConfigDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}