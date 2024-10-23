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

public class CreateDocumentsCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateDocumentsCommand, ResponseModel<DocumentsDto>>
{
    public async Task<ResponseModel<DocumentsDto>> Handle(CreateDocumentsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Documents = mapper.Map<dotnetcoreproject.Domain.Entities.Documents>(request);
            await unitOfWork.Repository<dotnetcoreproject.Domain.Entities.Documents>().AddAsync(Documents, cancellationToken);
            await unitOfWork.CompleteAsync();

            var DocumentsDto = mapper.Map<DocumentsDto>(Documents);
            return ResponseModel<DocumentsDto>.Success(DocumentsDto, "Documents created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating Documents.");
            return ResponseModel<DocumentsDto>.Failure($"Error creating Documents: {ex.Message}");
        }
    }
}