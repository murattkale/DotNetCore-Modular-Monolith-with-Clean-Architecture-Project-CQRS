using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Langs.Dtos;
using AutoMapper;
using Domain.Helpers;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Features.Langs.Commands;

public class PatchLangCommandHandler(
    ILangRepository LangRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchLangCommand, ResponseModel<LangDto>>
{
    public async Task<ResponseModel<LangDto>> Handle(PatchLangCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Lang =
                await LangRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(Lang);

            await LangRepository.UpdateAsync(Lang, cancellationToken);

            var LangDto = mapper.Map<LangDto>(Lang);
            return ResponseModel<LangDto>.Success(LangDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<LangDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}