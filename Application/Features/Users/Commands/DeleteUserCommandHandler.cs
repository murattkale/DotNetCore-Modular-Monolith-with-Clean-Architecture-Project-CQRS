using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Users.Commands;

public class DeleteUserCommandHandler(
    IUserRepository UserRepository,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<DeleteUserCommand, ResponseModel<Unit>>
{
    public async Task<ResponseModel<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UserRepository.DeleteByIdAsync(request.Id, cancellationToken);
            return ResponseModel<Unit>.Success(Unit.Value, "User deleted successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, $"Error deleting User with Id: {request.Id}");
            return ResponseModel<Unit>.Failure($"Error deleting User: {ex.Message}");
        }
    }
}