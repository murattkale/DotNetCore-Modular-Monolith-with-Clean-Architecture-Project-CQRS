using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Forms.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Forms.Commands;

public class PatchFormsCommandHandler(
    IFormsRepository FormsRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchFormsCommand, ResponseModel<FormsDto>>
{
    public async Task<ResponseModel<FormsDto>> Handle(PatchFormsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Forms =
                await FormsRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(Forms);

            await FormsRepository.UpdateAsync(Forms, cancellationToken);

            var FormsDto = mapper.Map<FormsDto>(Forms);
            return ResponseModel<FormsDto>.Success(FormsDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<FormsDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}