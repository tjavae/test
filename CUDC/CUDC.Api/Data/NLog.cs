using System;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CUDC.Api.Data
{
    [Table("CUDC_NLog")]
    public class NLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MachineName { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
