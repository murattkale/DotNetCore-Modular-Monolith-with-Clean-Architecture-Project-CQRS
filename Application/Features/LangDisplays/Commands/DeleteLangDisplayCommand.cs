using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.LangDisplays.Commands;

public class DeleteLangDisplayCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}