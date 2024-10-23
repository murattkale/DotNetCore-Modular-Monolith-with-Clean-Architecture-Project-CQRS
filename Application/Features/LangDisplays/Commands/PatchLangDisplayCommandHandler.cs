using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.LangDisplays.Dtos;
using AutoMapper;
using Domain.Helpers;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Features.LangDisplays.Commands;

public class PatchLangDisplayCommandHandler(
    ILangDisplayRepository LangDisplayRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchLangDisplayCommand, ResponseModel<LangDisplayDto>>
{
    public async Task<ResponseModel<LangDisplayDto>> Handle(PatchLangDisplayCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var LangDisplay =
                await LangDisplayRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(LangDisplay);

            await LangDisplayRepository.UpdateAsync(LangDisplay, cancellationToken);

            var LangDisplayDto = mapper.Map<LangDisplayDto>(LangDisplay);
            return ResponseModel<LangDisplayDto>.Success(LangDisplayDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<LangDisplayDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}