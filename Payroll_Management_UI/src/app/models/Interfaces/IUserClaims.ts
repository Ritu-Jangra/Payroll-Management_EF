import { PermissionType } from "../Enums/PermissionsType";
import { RolesType } from "../Enums/RolesType";

export interface IUserClaims {
  role: RolesType;
  permissions: PermissionType[];
}