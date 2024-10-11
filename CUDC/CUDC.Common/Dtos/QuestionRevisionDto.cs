using CUDC.Common.Enums;
using System;

namespace CUDC.Common.Dtos
{
    public class QuestionRevisionDto
    {
        public Guid? Id { get; set; }

        public QuestionType Type { get; set; }
        
        public string TypeString { get;  set; }

        public string Text { get; set; }

        public int? MaxLength { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
