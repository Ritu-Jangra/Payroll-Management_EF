import { DepartmentType } from "../Enums/DepartmentType";
import { RolesType } from "../Enums/RolesType";

export interface IEmployee {
    userName: string;
    firstName: string;
    lastName?: string;
    email: string;
    department: DepartmentType;
    salary: number;
    role: RolesType;
    managerName?: string;
}