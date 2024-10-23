using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.SiteConfigs.Commands;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.SiteConfigs.Commands;

public class DeleteSiteConfigCommandHandler(
    ISiteConfigRepository SiteConfigRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteSiteConfigCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteSiteConfigCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await SiteConfigRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "SiteConfig deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting SiteConfig with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting SiteConfig: {ex.Message}");
        }
    }
}