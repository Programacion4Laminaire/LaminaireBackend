using ResultCouriers.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResultCouriers.Application.Interfaces
{
    public interface IResultCourierRPA
    {
       public Task<ResultCourierResponseRPA> SendCallAsync(SendResultCourierRPADto command);
    }
}
