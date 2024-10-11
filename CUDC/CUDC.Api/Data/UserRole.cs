using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CUDC.Api.Data
{
    [Table("CUDC_UserRoles")]
    public class UserRole
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid UniqueId { get; set; }

        public string UserId { get; set; }

        public int RoleId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
