import { PermissionType } from "../Enums/PermissionsType";
import { RolesType } from "../Enums/RolesType";

export interface IMenuItem {
  label: string;
  route: string;
  roles: RolesType[];
  permissions: PermissionType[];
}