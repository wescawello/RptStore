import { IUser } from './IUser';

export class User implements IUser {
    id?: number;
    username: string;
    password: string;
    firstName?: string;
    lastName?: string;
    token?: string;
}
export class UserProfile extends User  {
 
  Photo: string;
   
 
}
