using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.FormTypes.Dtos;
using AutoMapper;
using Domain.Helpers;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Features.FormTypes.Commands;

public class PatchFormTypeCommandHandler(
    IFormTypeRepository FormTypeRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchFormTypeCommand, ResponseModel<FormTypeDto>>
{
    public async Task<ResponseModel<FormTypeDto>> Handle(PatchFormTypeCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var FormType =
                await FormTypeRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(FormType);

            await FormTypeRepository.UpdateAsync(FormType, cancellationToken);

            var FormTypeDto = mapper.Map<FormTypeDto>(FormType);
            return ResponseModel<FormTypeDto>.Success(FormTypeDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<FormTypeDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}