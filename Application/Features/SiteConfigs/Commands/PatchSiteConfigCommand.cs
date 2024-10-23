using dotnetcoreproject.Domain;
using dotnetcoreproject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Features.SiteConfigs.Commands;

public class PatchSiteConfigCommand : IRequest<ResponseModel<SiteConfigDto>>
{
    public int Id { get; set; }
    public JsonPatchDocument<SiteConfig> PatchDocument { get; set; }
}