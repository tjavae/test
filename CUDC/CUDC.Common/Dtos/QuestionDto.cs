using System;

namespace CUDC.Common.Dtos
{
    public class QuestionDto
    {
        public QuestionDto()
        {
            Revision = new QuestionRevisionDto();
        }

        public Guid Id { get; set; }

        public Guid? SectionId { get; set; }
        public int SectionPos { get; set; }

        public string Number { get; set; }

        public int Position { get; set; }

        public bool IsRequired { get; set; }

        public QuestionRevisionDto Revision { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
