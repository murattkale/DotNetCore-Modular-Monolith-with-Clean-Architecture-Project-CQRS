#nullable enable
using dotnetcoreproject.Domain;

namespace Application.Features.Users.Dtos;

public class UserDto : BaseEntity
{
    public string Mail { get; set; }
    public string Pass { get; set; }
    public string UserName { get; set; }
   
}