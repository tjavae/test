using System;
using System.Collections.Generic;


namespace CUDC.Common.Dtos
{
    public class QuestionsEdit
    {
        public Guid SurveyId { get; set; }

        public IEnumerable<SectionDto> Sections { get; set; }

        public IEnumerable<QuestionDto> Questions { get; set; }

        public IEnumerable<QuestionOptionDto> QuestionOptions { get; set; }

        public IEnumerable<QuestionReferenceDto> QuestionReferences { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
