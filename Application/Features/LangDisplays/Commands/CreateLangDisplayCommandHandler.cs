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

namespace Application.Features.LangDisplays.Commands;

public class CreateLangDisplayCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateLangDisplayCommand, ResponseModel<LangDisplayDto>>
{
    public async Task<ResponseModel<LangDisplayDto>> Handle(CreateLangDisplayCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var LangDisplay = mapper.Map<LangDisplay>(request);
            await unitOfWork.Repository<LangDisplay>().AddAsync(LangDisplay);
            await unitOfWork.CompleteAsync();

            var LangDisplayDto = mapper.Map<LangDisplayDto>(LangDisplay);
            return ResponseModel<LangDisplayDto>.Success(LangDisplayDto, "LangDisplay created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating LangDisplay.");
            return ResponseModel<LangDisplayDto>.Failure($"Error creating LangDisplay: {ex.Message}");
        }
    }
}