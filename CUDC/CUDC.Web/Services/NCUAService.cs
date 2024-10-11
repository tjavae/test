using CUDC.Common.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CUDC.Web.Services
{
    public class NCUAService: ServiceBase, INCUAService
    {

        private readonly IConfiguration _config;

        public NCUAService(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public async Task<IEnumerable<SelectListItem>> GetStates()
        {

            var requestUri = NCUAApiUrl();
            var states = await GetAsync<IEnumerable<StateDto>>(requestUri);
            var convertStates = new List<SelectListItem>();
            foreach (var state in states)
            {
                convertStates.Add(new SelectListItem { Text = state.StateName, Value = state.AlphaCode });
            }
            return convertStates;

        }

        private string NCUAApiUrl()
        {
            return $"{_config["NCUAAPIUrl"]}{"States"}";
        }
    }
}
