using CUDC.Common.Dtos.CuSearch;
using System;
using System.Collections.Generic;

namespace CUDC.Common.Dtos
{
    public class SurveyInfo
    {
        public BasicInfo BasicInfo { get; set; }

        public SurveyDto Survey { get; set; }

        public IEnumerable<SectionDto> Sections { get; set; }

        public IEnumerable<QuestionDto> Questions { get; set; }

        public IEnumerable<QuestionOptionDto> QuestionOptions { get; set; }

        public ResponseDto Answers { get; set; }

        public IEnumerable<QuestionReferenceDto> QuestionReferences { get; set; }

        public Boolean HasPreSubmitedSurvey { get; set; }

    }
}
