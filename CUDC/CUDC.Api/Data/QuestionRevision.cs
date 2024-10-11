using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("CUDC_QuestionRevisions")]
    public class QuestionRevision
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int QuestionId { get; set; }

        public int QuestionTypeId { get; set; }

        public string Text { get; set; }

        public int? MaxLength { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}
