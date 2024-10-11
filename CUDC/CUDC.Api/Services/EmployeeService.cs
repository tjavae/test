using CUDC.Api.Data;
using CUDC.Common.Dtos.UserSearch;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Api.Services
{
    public class EmployeeService : ServiceBase, IEmployeeService
    {
        private readonly IConfiguration _config;
        
        public EmployeeService(IConfiguration config) : base(config)
        {
            _config = config;
        }
        public async Task<IEnumerable<EmployeeDto>> GetLimitedEmployees(int? offset, int? size)
        {
            var requestUri = EmployeeApiUrl($"{offset}/{size}/GetAllEmployees");

            return await GetAsync<List<EmployeeDto>>(requestUri);
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployees()
        {
            var requestUri = EmployeeApiUrl("GetAllEmployees");

            return await GetAsync<List<EmployeeDto>>(requestUri);
        }

        public async Task<EmployeeDto> GetEmployeeByLanId(string lanId)
        {
            var requestUri = EmployeeApiUrl($"employee/{lanId}");

            return await GetAsync<EmployeeDto>(requestUri);
        }

        public async Task<EmployeeDto> GetEmployeeByNumber(string employeeNumber)
        {
            //var requestUri = EmployeeApiUrl($"GetByEmployeeNumber?employeeNumber={employeeNumber}");
            var requestUri = EmployeeApiUrl($"{employeeNumber}");

            return await GetAsync<EmployeeDto>(requestUri);
        }
        private string EmployeeApiUrl(string path)
        {
            return $"{_config["EmployeeAPIUrl"]}{path}";
        }

        public async Task<IEnumerable<SearchUserResult>> SearchEmployeeByRequest(SearchUserRequest req)
        {
            var requestUri = EmployeeApiUrl("SearchEmployeeByRequest");

            return await PostAsync<List<SearchUserResult>>(requestUri, req);
        }
        
    }
}
