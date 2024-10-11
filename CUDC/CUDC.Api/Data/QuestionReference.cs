using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace CUDC.Api.Data
{
    [Table("CUDC_QuestionReferences")]
    public class QuestionReference
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public Guid ReferenceQuestionId { get; set; }

        public Guid ReferenceOptionId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid QuestionOptionId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
