using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Documents.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Documents.Commands;

public class PatchDocumentsCommandHandler(
    IDocumentsRepository DocumentsRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchDocumentsCommand, ResponseModel<DocumentsDto>>
{
    public async Task<ResponseModel<DocumentsDto>> Handle(PatchDocumentsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Documents =
                await DocumentsRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyTo(Documents);

            await DocumentsRepository.UpdateAsync(Documents, cancellationToken);

            var DocumentsDto = mapper.Map<DocumentsDto>(Documents);
            return ResponseModel<DocumentsDto>.Success(DocumentsDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<DocumentsDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}