export class UserTypeDef{
    userType: string;
    canAccess: boolean;
    district: number;
    region: string;
    se: string;
    districts: string[];
    regions:string[];
    ses:string[];
    reviewCat: boolean;
    editCat: boolean;
    reviewSe: boolean;
    editSe: boolean;
    reviewDos: boolean;
    editDos: boolean;
}
export class UserPermission{
    action: string;
    groupName: string;
    module: string;
    region: number;
    se: string;
    district: number;
    creditUnion: number;
}