using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.FormTypes.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.FormTypes.Queries;

public class
    GetFormTypeByIdQueryHandler : IRequestHandler<GetFormTypeByIdQuery, ResponseModel<FormTypeDto>>
{
    private readonly IFormTypeRepository _FormTypeRepository;
    private readonly IErrorHandlingService _errorHandlingService;
    private readonly IMapper _mapper;

    public GetFormTypeByIdQueryHandler(IFormTypeRepository FormTypeRepository, IMapper mapper,
        IErrorHandlingService errorHandlingService)
    {
        _FormTypeRepository = FormTypeRepository;
        _mapper = mapper;
        _errorHandlingService = errorHandlingService;
    }

    public async Task<ResponseModel<FormTypeDto>> Handle(GetFormTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var FormType = await _FormTypeRepository.GetByIdAsync(
                request.Id,
                request.Cache,
                true,
                cancellationToken);

            if (FormType == null) return ResponseModel<FormTypeDto>.Failure("Content page not found.");

            var FormTypeDto = _mapper.Map<FormTypeDto>(FormType);
            return ResponseModel<FormTypeDto>.Success(FormTypeDto, "Content page retrieved successfully.");
        }
        catch (Exception ex)
        {
            _errorHandlingService.LogError(ex, "Error retrieving content page by id.");
            return ResponseModel<FormTypeDto>.Failure($"Error retrieving content page: {ex.Message}");
        }
    }
}