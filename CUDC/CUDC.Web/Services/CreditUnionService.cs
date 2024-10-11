using CUDC.Common.Dtos.CuSearch;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public class CreditUnionService : ServiceBase, ICreditUnionService
    {
        private readonly IConfiguration _config;

        public CreditUnionService(IConfiguration config) : base(config)
        {
            _config = config;
        }
        
        public async Task<BasicInfo> GetCreditUnionBasicInfo(int charterNumber)
        {

            var requestUri = CreditUnionApiUrl($"GetCreditUnionByNumber?cu_number={charterNumber}");
            var cu = await GetAsync<CreditUnionDto>(requestUri);
            BasicInfo basicInfo = new()
            {
                CharterNumber = cu.CuNumber,
                JoinNumber = cu.JoinNumber,
                Name = cu.CuName
            };

            return basicInfo;
        }

        public async Task<CUInformationDto> GetCreditUnionByNumber(int creditUnionNumber)
        {

            var requestUri = CreditUnionApiUrl($"GetCreditUnionByNumber?cu_number={creditUnionNumber}");
            //return await GetAsync<CreditUnionDto>(requestUri);

            var cu = await GetAsync<CreditUnionDto>(requestUri);
            var cuInfo = new CUInformationDto
            {
                CuNumber = cu.CuNumber,
                JoinNumber = cu.JoinNumber,
                ActualState = cu.ActualState,
                Region = cu.Region,
                SE = cu.SeCode,
                CountyCode = (int)cu.CountyCode,
                District = (int)cu.District
            };

            return cuInfo;
        }

        private string CreditUnionApiUrl(string path)
        {
            return $"{_config["CreditUnionAPIUrl"]}{path}";
        }

        public async Task<IEnumerable<SearchResult>> SearchCreditUnionsByRequest(SearchRequest request)
        {
            var requestUri = CreditUnionApiUrl($"SearchCreditUnionsByRequest");            
            var results = await PostAsync<List<SearchResult>>(requestUri, request);
            if (results == null ) 
            {
                return default;//make sure return a not null value.
            }
            return results;            
        }        
    }
}
