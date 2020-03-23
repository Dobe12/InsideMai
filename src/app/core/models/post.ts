import {Department} from "./department";
import {User} from "./user";

export interface  Post {
  id: number;
  title: string;
  content: string;
  publishDate: string;
  likesCount: number;
  savesCount: number;
  commentsCount: number;
  author: User;
  department: Department;
}
