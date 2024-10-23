using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Queries;

public class GetSiteConfigByIdQuery : IRequest<ResponseModel<SiteConfigDto>>
{
    public GetSiteConfigByIdQuery(int id)
    {
        Id = id;
    }

    public GetSiteConfigByIdQuery()
    {
    }

    public required int Id { get; set; }

    public required bool Cache { get; set; }
}