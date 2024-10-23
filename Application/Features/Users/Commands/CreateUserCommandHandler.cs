using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Users.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands;

public class CreateUserCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateUserCommand, ResponseModel<UserDto>>
{
    public async Task<ResponseModel<UserDto>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var User = mapper.Map<User>(request);
            await unitOfWork.Repository<User>().AddAsync(User);
            await unitOfWork.CompleteAsync();

            var UserDto = mapper.Map<UserDto>(User);
            return ResponseModel<UserDto>.Success(UserDto, "User created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating User.");
            return ResponseModel<UserDto>.Failure($"Error creating User: {ex.Message}");
        }
    }
}