using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.FormTypes.Commands;

public class DeleteFormTypeCommandHandler(
    IFormTypeRepository FormTypeRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteFormTypeCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteFormTypeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await FormTypeRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "FormType deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting FormType with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting FormType: {ex.Message}");
        }
    }
}