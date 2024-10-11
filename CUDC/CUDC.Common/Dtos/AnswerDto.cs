using System;

namespace CUDC.Common.Dtos
{
    public class AnswerDto
    {
        public Guid QuestionId { get; set; }

        public Guid? QuestionOptionId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
