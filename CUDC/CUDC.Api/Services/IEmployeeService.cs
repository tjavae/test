using CUDC.Api.Data;
using CUDC.Common.Dtos.UserSearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public interface IEmployeeService
    {
        public Task<EmployeeDto> GetEmployeeByNumber(string employeeNumber);
        public Task<EmployeeDto> GetEmployeeByLanId(string lanId);
        public Task<IEnumerable<EmployeeDto>> GetLimitedEmployees(int? offset, int? size);
        public Task<IEnumerable<EmployeeDto>> GetAllEmployees();
        public Task<IEnumerable<SearchUserResult>> SearchEmployeeByRequest(SearchUserRequest req);
    }
}
