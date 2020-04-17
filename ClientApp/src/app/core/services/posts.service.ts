import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DataService} from "./data.service";
import {DepartmentLevels, Post, PostType} from "../models/post";
import {environment} from "../../../environments/environment";
import {BehaviorSubject} from "rxjs";
import {User} from "../models/user";
import {debounceTime, delay, distinctUntilChanged} from "rxjs/operators";
import {Router} from "@angular/router";

@Injectable()
export class PostsService extends DataService {
  type: number;
  departmentLevel: number;

  private postsSubject = new BehaviorSubject<Post[]>({} as Post[]);
  public posts = this.postsSubject.asObservable().pipe(distinctUntilChanged());

  constructor(http: HttpClient, private router: Router) {
    super('posts', http);

    this.type = DepartmentLevels.Mai;
    this.departmentLevel = PostType.All;
  }

  public get postsValue(): Post[] {
    return this.postsSubject.value;
  }

  getUserPosts(userId) {
    return this.http.get(this.url + '/user/' + userId);
  }

  getUserFavPosts(userId) {
    return this.http.get(this.url + '/user/' + userId + '/fav');
  }

  searchPosts(terms: string) {
    console.log(terms);
    if (terms === null || terms === '') {
      this.applyFilters();
      return;
    }

    return this.http.get(this.url + '/search/' + terms).pipe(debounceTime(10000)).subscribe(
      response =>  {
        this.postsSubject.next(response as Post[]);
      });
  }

  getPostComments(postId) {
    return this.http.get(this.url + '/' + postId + '/comments');
  }

  getPostsDepartmentByLevel(userDepartment, departmentLvl) {
    return this.http.get(this.url + '/byDepartment/' + userDepartment + '/' + departmentLvl);
  }

  getPostsByType(type: PostType) {
    return this.http.get(this.url + /type/ + type);
  }

  applyFilters() {
    return this.http.get(this.url + `/filter/${this.type}/${this.departmentLevel}`).subscribe(
      response =>  {

        this.postsSubject.next(response as Post[]);
        this.redirect();
      });
  }

  private redirect() {
    const type = PostType[this.type].toLowerCase();
    const departmentLevel = PostType[this.departmentLevel].toLowerCase();

    this.router.navigate(['/', type, departmentLevel]);
  }

  addCommentOnPost(postId, comment) {
    console.log(postId);
    console.log(comment);
    return this.http.post(this.url + '/' + postId + '/addComment', JSON.stringify(comment));
  }
}
