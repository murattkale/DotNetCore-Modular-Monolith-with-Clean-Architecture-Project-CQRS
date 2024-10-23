using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.SiteConfigs.Commands;

public class DeleteSiteConfigCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}