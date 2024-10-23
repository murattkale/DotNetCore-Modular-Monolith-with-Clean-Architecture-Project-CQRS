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

public class CreateFormsCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateFormsCommand, ResponseModel<FormsDto>>
{
    public async Task<ResponseModel<FormsDto>> Handle(CreateFormsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Forms = mapper.Map<Domain.Entities.Forms>(request);
            await unitOfWork.Repository<Domain.Entities.Forms>().AddAsync(Forms);
            await unitOfWork.CompleteAsync();

            var FormsDto = mapper.Map<FormsDto>(Forms);
            return ResponseModel<FormsDto>.Success(FormsDto, "Forms created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating Forms.");
            return ResponseModel<FormsDto>.Failure($"Error creating Forms: {ex.Message}");
        }
    }
}