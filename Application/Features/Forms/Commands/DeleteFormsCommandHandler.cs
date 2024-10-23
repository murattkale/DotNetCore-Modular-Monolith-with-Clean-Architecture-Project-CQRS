using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Commands;

public class DeleteFormsCommandHandler(
    IFormsRepository FormsRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteFormsCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteFormsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await FormsRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "Forms deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting Forms with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting Forms: {ex.Message}");
        }
    }
}