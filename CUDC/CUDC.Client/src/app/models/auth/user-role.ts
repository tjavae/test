import { Role } from './role';
import { UserPermission, UserTypeDef } from './user-type';
export class UserRole {
    id: string;
    userId: string;
    role: Role;
    roleString: string;
    specialRole: string;
    userType: UserTypeDef;
    createdOn: Date;
    createdBy: string;
    modifiedOn: Date;
    modifiedBy: string;
    permissions: UserPermission[];
    firstName: string;
    lastName: string;
    employeeNumber: string;
}
