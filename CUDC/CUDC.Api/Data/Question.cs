using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace CUDC.Api.Data
{
    [Table("CUDC_Questions")]
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int SurveyId { get; set; }

        public int? SectionId { get; set; }

        public string Number { get; set; }

        public int Position { get; set; }

        public bool IsRequired { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }
    }
}
