using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("CUDC_QuestionOptions")]
    public class QuestionOption
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int QuestionRevisionId { get; set; }

        public string Text { get; set; }

        public int Position { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("QuestionRevisionId")]
        public QuestionRevision QuestionRevision { get; set; }
    }
}
