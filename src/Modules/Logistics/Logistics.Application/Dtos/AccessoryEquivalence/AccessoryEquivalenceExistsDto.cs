using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Application.Dtos.AccessoryEquivalence
{
    public record AccessoryEquivalenceExistsDto
    {
        public string CodigoMP { get; init; } = default!;
        public string CodigoPT { get; init; } = default!;
        public int? ExcludeId { get; init; }  
    }
}
