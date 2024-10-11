using System;

namespace CUDC.Common.Dtos
{
    public class QuestionOptionDto
    {
        public Guid? Id { get; set; }

        public Guid QuestionId { get; set; }

        public string Text { get; set; }

        public int Position { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
