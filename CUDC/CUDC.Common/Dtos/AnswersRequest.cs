using System;

namespace CUDC.Common.Dtos
{
    public class AnswersRequest
    {
        public Guid SurveyId { get; set; }

        public string UserId { get; set; }

        public int CuNumber { get; set; }

        public int JoinNumber { get; set; }
    }
}
