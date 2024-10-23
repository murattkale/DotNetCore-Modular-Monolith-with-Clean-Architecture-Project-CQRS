using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Langs.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Queries;

public class
    GetLangByIdQueryHandler : IRequestHandler<GetLangByIdQuery, ResponseModel<LangDto>>
{
    private readonly ILangRepository _LangRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetLangByIdQueryHandler(ILangRepository LangRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _LangRepository = LangRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<LangDto>> Handle(GetLangByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Lang = await _LangRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (Lang == null) return ResponseModel<LangDto>.Failure("Content page not found.");

            var LangDto = _mapper.Map<LangDto>(Lang);
            return ResponseModel<LangDto>.Success(LangDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<LangDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}