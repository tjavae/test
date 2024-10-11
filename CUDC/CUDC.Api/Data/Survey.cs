using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("CUDC_Surveys")]
    public class Survey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? SurveyTypeId { get; set; }

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
