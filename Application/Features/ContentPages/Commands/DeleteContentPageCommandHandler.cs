using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.ContentPages.Commands;

public class DeleteContentPageCommandHandler(
    IContentPageRepository contentPageRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteContentPageCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteContentPageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await contentPageRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "ContentPage deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting contentpage with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting contentpage: {ex.Message}");
        }
    }
}