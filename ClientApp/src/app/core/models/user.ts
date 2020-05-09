import {Department} from "./department";
import {Post} from "./post";

export class  User {
  id: number;
  email: string;
  userPic: string;
  department: Department;
  posts: Post[];
  fullName: string;
  role: Roles;
  isSubscribe?: boolean;
  notificationsCount: number;
}

export enum Roles {
  Admin,
  User,
  Lecturer,
  Employee,
  Student
}
