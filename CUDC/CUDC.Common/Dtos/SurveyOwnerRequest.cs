using System;

namespace CUDC.Common.Dtos
{
    public class SurveyOwnerRequest
    {
        public Guid SurveyId { get; set; }

        public int CuNumber { get; set; }

        public string UserId { get; set; }
    }
}
