using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Commands;

public class DeleteDocumentsCommandHandler(
    IDocumentsRepository DocumentsRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteDocumentsCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteDocumentsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await DocumentsRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "Documents deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting Documents with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting Documents: {ex.Message}");
        }
    }
}