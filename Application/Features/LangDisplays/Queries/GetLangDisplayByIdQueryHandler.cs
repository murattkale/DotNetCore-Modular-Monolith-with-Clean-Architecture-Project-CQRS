using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.LangDisplays.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.LangDisplays.Queries;

public class
    GetLangDisplayByIdQueryHandler : IRequestHandler<GetLangDisplayByIdQuery, ResponseModel<LangDisplayDto>>
{
    private readonly ILangDisplayRepository _LangDisplayRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetLangDisplayByIdQueryHandler(ILangDisplayRepository LangDisplayRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _LangDisplayRepository = LangDisplayRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<LangDisplayDto>> Handle(GetLangDisplayByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var LangDisplay = await _LangDisplayRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (LangDisplay == null) return ResponseModel<LangDisplayDto>.Failure("Content page not found.");

            var LangDisplayDto = _mapper.Map<LangDisplayDto>(LangDisplay);
            return ResponseModel<LangDisplayDto>.Success(LangDisplayDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<LangDisplayDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}