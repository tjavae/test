using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace CUDC.Api.Data
{
    [Table("CUDC_Responses")]
    public class Response
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int SurveyId { get; set; }

        public string UserId { get; set; }

        public int CuNumber { get; set; }

        public int JoinNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? SubmittedOn { get; set; }

        public bool? IsRejected { get; set; }
    }
}
