using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace CUDC.Api.Data
{
    [Table("CUDC_Answers")]
    public class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int ResponseId { get; set; }

        public int QuestionRevisionId { get; set; }

        public int? QuestionOptionId { get; set; }

        public string Text { get; set; }

        public string LongText { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("QuestionOptionId")]
        public QuestionOption QuestionOption { get; set; }
    }
}
