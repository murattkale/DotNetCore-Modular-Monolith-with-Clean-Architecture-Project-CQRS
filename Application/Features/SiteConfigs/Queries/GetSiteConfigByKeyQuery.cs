using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Queries;

public class GetSiteConfigByKeyQuery : IRequest<ResponseModel<SiteConfigDto>>
{
    public GetSiteConfigByKeyQuery(string ConfigKey)
    {
        ConfigKey = ConfigKey;
    }

    public GetSiteConfigByKeyQuery()
    {
    }

    public required string ConfigKey { get; set; }

    public required bool Cache { get; set; }
}