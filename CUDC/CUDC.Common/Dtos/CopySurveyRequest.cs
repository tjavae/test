using System;

namespace CUDC.Common.Dtos
{
    public class CopySurveyRequest
    {
        public Guid SurveyId { get; set; }

        public string UserId { get; set; }
    }
}
