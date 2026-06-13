import { Rol } from "./rol.interface";

export interface User {
  userId?: number;
  fullname?: string;
  username: string;
  password?: string;
  correo?: string;
  telefono?: string;
  estado?: string;
  roles?: Rol[];
  token?: string;
}