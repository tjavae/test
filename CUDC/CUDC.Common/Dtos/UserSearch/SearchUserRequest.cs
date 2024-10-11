using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUDC.Common.Dtos.UserSearch
{
    public class SearchUserRequest
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
                        
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("employeeNumber")]
        public string EmployeeNumber { get; set; }
    }
}
