using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Helpers;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Features.Users.Commands;

public class PatchUserCommandHandler(
    IUserRepository UserRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchUserCommand, ResponseModel<UserDto>>
{
    public async Task<ResponseModel<UserDto>> Handle(PatchUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var User =
                await UserRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(User);

            await UserRepository.UpdateAsync(User, cancellationToken);

            var UserDto = mapper.Map<UserDto>(User);
            return ResponseModel<UserDto>.Success(UserDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<UserDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}