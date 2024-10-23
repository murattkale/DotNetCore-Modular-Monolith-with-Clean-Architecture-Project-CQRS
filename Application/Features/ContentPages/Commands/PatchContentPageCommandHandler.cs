using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.ContentPages.Dtos;
using AutoMapper;
using Domain.Helpers;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Features.ContentPages.Commands;

public class PatchContentPageCommandHandler(
    IContentPageRepository contentPageRepository,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<PatchContentPageCommand, ResponseModel<ContentPageDto>>
{
    public async Task<ResponseModel<ContentPageDto>> Handle(PatchContentPageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var contentPage =
                await contentPageRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            request.PatchDocument.ApplyCustomOperation(contentPage, "/Name", OperationType.Replace,
                (entity, value) =>
                {
                    if (entity.Link != null) entity.Link = entity.Link.GenerateSlug();
                });

            request.PatchDocument.ApplyTo(contentPage);

            await contentPageRepository.UpdateAsync(contentPage, cancellationToken);

            var contentPageDto = mapper.Map<ContentPageDto>(contentPage);
            return ResponseModel<ContentPageDto>.Success(contentPageDto, "Content page updated successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error updating content page.");
            return ResponseModel<ContentPageDto>.Failure($"Error updating content page: {ex.Message}");
        }
    }
}