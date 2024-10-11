using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [Column("EMPLOYEE_NUM")]
        public string EmployeeNumber { get; set; }

        [Column("FIRST_NAME")]
        public string FirstName { get; set; }

        [Column("LAST_NAME")]
        public string LastName { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("GS_GRADE")]
        public string GsGrade { get; set; }

        [Column("EMPLOYEE_TYPE")]
        public string EmployeeType { get; set; }

        [Column("OFFICE")]
        public string Office { get; set; }

        [Column("REGION")]
        public string Region { get; set; }

        [Column("SE")]
        public string Se { get; set; }

        [Column("Mail_Box")]
        public string MailBox { get; set; }
    }
}
