#nullable enable
using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;

namespace Application.Features.SiteConfigs.Commands;

public class CreateSiteConfigCommand : IRequest<ResponseModel<SiteConfigDto>>
{
    public required string ConfigKey { get; set; }
    public required string ConfigValue { get; set; }
}