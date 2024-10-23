using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Commands;

public class CreateSiteConfigCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateSiteConfigCommand, ResponseModel<SiteConfigDto>>
{
    public async Task<ResponseModel<SiteConfigDto>> Handle(CreateSiteConfigCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var SiteConfig = mapper.Map<SiteConfig>(request);
            await unitOfWork.Repository<SiteConfig>().AddAsync(SiteConfig);
            await unitOfWork.CompleteAsync();

            var SiteConfigDto = mapper.Map<SiteConfigDto>(SiteConfig);
            return ResponseModel<SiteConfigDto>.Success(SiteConfigDto, "SiteConfig created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating SiteConfig.");
            return ResponseModel<SiteConfigDto>.Failure($"Error creating SiteConfig: {ex.Message}");
        }
    }
}