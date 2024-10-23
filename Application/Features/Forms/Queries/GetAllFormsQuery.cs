using Application.Features.Forms.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.Forms.Queries;

public class GetAllFormsQuery : IRequest<ResponseModel<PagedResult<FormsDto>>>
{
    public PagedRequest<Domain.Entities.Forms> PagedRequest { get; set; }
}