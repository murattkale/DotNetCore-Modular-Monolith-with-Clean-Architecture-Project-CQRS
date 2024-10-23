#nullable enable
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Helpers;

namespace dotnetcoreproject.Domain.Entities;

public class SiteConfigDto : BaseEntity
{
    public string ConfigKey { get; set; }
    public string ConfigValue { get; set; }
   
}