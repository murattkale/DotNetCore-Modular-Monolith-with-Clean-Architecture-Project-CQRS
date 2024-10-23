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

namespace Application.Features.Users.Queries;

public class
    GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ResponseModel<UserDto>>
{
    private readonly IUserRepository _UserRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository UserRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _UserRepository = UserRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<UserDto>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var User = await _UserRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (User == null) return ResponseModel<UserDto>.Failure("Content page not found.");

            var UserDto = _mapper.Map<UserDto>(User);
            return ResponseModel<UserDto>.Success(UserDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<UserDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}