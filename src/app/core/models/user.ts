import {Department} from "./department";
import {Post} from "./post";

export interface  User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  userPic: string;
  department: Department;
  posts: Post[];
}
