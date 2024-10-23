using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Langs.Commands;

public class DeleteLangCommandHandler(
    ILangRepository LangRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteLangCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteLangCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await LangRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "Lang deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting Lang with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting Lang: {ex.Message}");
        }
    }
}