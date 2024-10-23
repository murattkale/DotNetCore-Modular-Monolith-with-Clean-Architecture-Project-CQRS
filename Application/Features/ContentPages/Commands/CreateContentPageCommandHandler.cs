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

namespace Application.Features.ContentPages.Commands;

public class CreateContentPageCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateContentPageCommand, ResponseModel<ContentPageDto>>
{
    public async Task<ResponseModel<ContentPageDto>> Handle(CreateContentPageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Link != null) request.Link = request.Link.GenerateSlug();
            var contentpage = mapper.Map<ContentPage>(request);
            await unitOfWork.Repository<ContentPage>().AddAsync(contentpage);
            await unitOfWork.CompleteAsync();

            var contentpageDto = mapper.Map<ContentPageDto>(contentpage);
            return ResponseModel<ContentPageDto>.Success(contentpageDto, "ContentPage created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating contentpage.");
            return ResponseModel<ContentPageDto>.Failure($"Error creating contentpage: {ex.Message}");
        }
    }
}