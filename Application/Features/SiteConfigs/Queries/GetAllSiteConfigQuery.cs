using Application.Features.Users.Dtos;
using Domain;
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Queries;

public class GetAllSiteConfigQuery : IRequest<ResponseModel<PagedResult<SiteConfigDto>>>
{
    public PagedRequest<SiteConfig> PagedRequest { get; set; }
}