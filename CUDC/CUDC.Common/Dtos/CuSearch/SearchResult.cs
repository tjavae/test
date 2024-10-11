using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CUDC.Common.Dtos.CuSearch
{
    public class SearchResult
    {   
        public string Name { get; set; }
             
        public int CharterNumber { get; set; }
                
        public int JoinNumber { get; set; }
    }
}
