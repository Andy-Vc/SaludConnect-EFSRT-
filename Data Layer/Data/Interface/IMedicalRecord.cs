using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.DTO;

namespace Data.Interface
{
    public interface IMedicalRecord
    {
        Task<ResultResponse<int>> RegisterRecordWithServicesAsync(MedicalRecord record);
    }
}
