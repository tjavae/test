using System;

namespace CUDC.Common.Dtos
{
    public class QuestionReferenceDto
    {
        public Guid? Id { get; set; }

        public Guid ReferenceQuestionId { get; set; }

        public Guid ReferenceOptionId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid QuestionOptionId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
