using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.SiteConfigs.Commands;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Commands;

public class PatchSiteConfigCommandHandler(
    ISiteConfigRepository SiteConfigRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchSiteConfigCommand, ResponseModel<SiteConfigDto>>
{
    public async Task<ResponseModel<SiteConfigDto>> Handle(PatchSiteConfigCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var SiteConfig =
                await SiteConfigRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(SiteConfig);

            await SiteConfigRepository.UpdateAsync(SiteConfig, cancellationToken);

            var SiteConfigDto = mapper.Map<SiteConfigDto>(SiteConfig);
            return ResponseModel<SiteConfigDto>.Success(SiteConfigDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<SiteConfigDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}