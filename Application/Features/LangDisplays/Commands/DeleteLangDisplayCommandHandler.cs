using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.LangDisplays.Commands;

public class DeleteLangDisplayCommandHandler(
    ILangDisplayRepository LangDisplayRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteLangDisplayCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteLangDisplayCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await LangDisplayRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "LangDisplay deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting LangDisplay with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting LangDisplay: {ex.Message}");
        }
    }
}