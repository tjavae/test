using CUDC.Common.Dtos.CuSearch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public interface ICreditUnionService
    {
        Task<BasicInfo> GetCreditUnionBasicInfo(int charterNumber);
        Task<CUInformationDto> GetCreditUnionByNumber(int creditUnionNumber);
        Task<IEnumerable<SearchResult>> SearchCreditUnionsByRequest(SearchRequest request);
    }
}
