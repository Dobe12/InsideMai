import {Department} from "./department";
import {User} from "./user";

export interface  Post {
  id: number;
  title: string;
  content: string;
  publishDate: string;
  likesCount: number;
  savesCount: number;
  isAnonymous: boolean;
  commentsCount: number;
  author: User;
  department: Department;
  Type: PostType;
}

export enum PostType {
  All = 1,
  Question = 2,
  Article = 3,
  Advert = 4,
  Event = 5
}

export enum DepartmentLevels {
  Mai = 1,
  University = 2,
  Department = 3,
  Group= 4,
}
