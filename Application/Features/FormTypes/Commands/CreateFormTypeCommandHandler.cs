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

namespace Application.Features.FormTypes.Commands;

public class CreateFormTypeCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateFormTypeCommand, ResponseModel<FormTypeDto>>
{
    public async Task<ResponseModel<FormTypeDto>> Handle(CreateFormTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var FormType = mapper.Map<FormType>(request);
            await unitOfWork.Repository<FormType>().AddAsync(FormType);
            await unitOfWork.CompleteAsync();

            var FormTypeDto = mapper.Map<FormTypeDto>(FormType);
            return ResponseModel<FormTypeDto>.Success(FormTypeDto, "FormType created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating FormType.");
            return ResponseModel<FormTypeDto>.Failure($"Error creating FormType: {ex.Message}");
        }
    }
}