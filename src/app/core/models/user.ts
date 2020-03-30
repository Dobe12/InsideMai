import {Department} from "./department";
import {Post} from "./post";

export class  User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  userPic: string;
  department: Department;
  posts: Post[];
  //в будущем нна стороне бека сделать мапер, который будет отсылать еще и fuulname
  fullName: string;


}
