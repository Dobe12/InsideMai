import {User} from "./user";

export class Comment {
  id: number;
  content: string;
  publishDate: string;
  likesCount: number;
  commentsCount: number;
  author: User;
  likedByCurrentUser: boolean;
}
