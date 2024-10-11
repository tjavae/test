using CUDC.Common.Enums;
using System;

namespace CUDC.Common.Dtos
{
    public class UserRoleDto
    {
        public Guid? Id { get; set; }

        public string UserId { get; set; }

        public Role Role { get; set; }

        public string RoleString { get;  set; }
        public string SpecialRole { get; set; }
        public UserTypeDef UserType { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }

        public UserPermission[] Permissions { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmployeeNumber { get; set; }
    }
}
