using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Entities
{
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
    }
}
