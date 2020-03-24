import {User} from "./user";

export interface Comment {
  id: number;
  content: string;
  publishDate: string;
  likesCount: number;
  commentsCount: number;
  author: User;
}
