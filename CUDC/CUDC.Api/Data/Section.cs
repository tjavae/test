﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("CUDC_Sections")]
    public class Section
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public int SurveyId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Position { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        [ForeignKey("SurveyId")]
        public Survey Survey { get; set; }
    }
}
