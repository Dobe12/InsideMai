import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DataService} from "./data.service";

@Injectable()
export class PostsService extends DataService {
  constructor(http: HttpClient) {
    super('posts', http);
  }

  getUserPost(userId) {
    return this.http.get(this.url + '/user/' + userId);
  }

  getPostComments(postId) {
    return this.http.get(this.url + '/' + postId + '/comments');
  }

  getPostsDepartmentByLevel(userDepartment, departmentLvl) {
    return this.http.get(this.url + '/byDepartment/' + userDepartment + '/' + departmentLvl);
  }

  addCommentOnPost(postId, comment) {
    console.log(postId);
    console.log(comment);
    return this.http.post(this.url + '/' + postId + '/addComment', JSON.stringify(comment));
  }
}
