using CUDC.Common.Enums;
using System;

namespace CUDC.Common.Dtos
{
    public class SurveyDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public SurveyType Type { get; set; }

        public string TypeString { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
        public string InformationText { get; set; }

    }
}
