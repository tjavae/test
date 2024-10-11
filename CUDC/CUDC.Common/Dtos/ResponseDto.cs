using System;
using System.Collections.Generic;

namespace CUDC.Common.Dtos
{
    public class ResponseDto
    {
        public ResponseDto()
        {
            Answers = new List<AnswerDto>();
        }

        public Guid Id { get; set; }

        public Guid SurveyId { get; set; }

        public string UserId { get; set; }

        public int CuNumber { get; set; }

        public int JoinNumber { get; set; }

        public List<AnswerDto> Answers { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public bool? IsRejected { get; set; }
    }
}
