using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Langs.Dtos;
using AutoMapper;
using Domain.Repositories;
using dotnetcoreproject.Application.Interfaces;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Langs.Commands;

public class CreateLangCommandHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IErrorHandlingService errorHandlingService)
    : IRequestHandler<CreateLangCommand, ResponseModel<LangDto>>
{
    public async Task<ResponseModel<LangDto>> Handle(CreateLangCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var Lang = mapper.Map<Lang>(request);
            await unitOfWork.Repository<Lang>().AddAsync(Lang);
            await unitOfWork.CompleteAsync();

            var LangDto = mapper.Map<LangDto>(Lang);
            return ResponseModel<LangDto>.Success(LangDto, "Lang created successfully.");
        }
        catch (Exception ex)
        {
            errorHandlingService.LogError(ex, "Error creating Lang.");
            return ResponseModel<LangDto>.Failure($"Error creating Lang: {ex.Message}");
        }
    }
}