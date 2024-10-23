using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Forms.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Queries;

public class
    GetFormsByIdQueryHandler : IRequestHandler<GetFormsByIdQuery, ResponseModel<FormsDto>>
{
    private readonly IFormsRepository _FormsRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetFormsByIdQueryHandler(IFormsRepository FormsRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _FormsRepository = FormsRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<FormsDto>> Handle(GetFormsByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Forms = await _FormsRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (Forms == null) return ResponseModel<FormsDto>.Failure("Content page not found.");

            var FormsDto = _mapper.Map<FormsDto>(Forms);
            return ResponseModel<FormsDto>.Success(FormsDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<FormsDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}