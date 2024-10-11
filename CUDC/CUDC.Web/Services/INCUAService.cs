using CUDC.Common.Dtos;
using CUDC.Common.Dtos.CuSearch;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface INCUAService
    {
        public Task<IEnumerable<SelectListItem>>  GetStates();
    }
}
