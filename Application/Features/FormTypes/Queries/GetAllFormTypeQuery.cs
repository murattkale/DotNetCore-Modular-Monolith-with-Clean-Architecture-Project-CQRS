using Application.Features.FormTypes.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.FormTypes.Queries;

public class GetAllFormTypeQuery : IRequest<ResponseModel<PagedResult<FormTypeDto>>>
{
    public PagedRequest<FormType> PagedRequest { get; set; }
}