using dotnetcoreproject.Domain;
using MediatR;

namespace Application.Features.Langs.Commands;

public class DeleteLangCommand : IRequest<ResponseModel<Unit>>
{
    public int Id { get; set; }
}