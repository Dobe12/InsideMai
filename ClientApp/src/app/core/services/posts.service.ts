import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {DataService} from "./data.service";
import {DepartmentLevels, Post, PostType} from "../models/post";
import {Comment} from "../models/comment";
import {environment} from "../../../environments/environment";
import {BehaviorSubject, Observable, of} from "rxjs";
import {User} from "../models/user";
import {debounceTime, delay, distinctUntilChanged, filter} from "rxjs/operators";
import {Router} from "@angular/router";

@Injectable()
export class PostsService extends DataService {
  type: number;
  departmentLevel: number;

  private postsSubject: BehaviorSubject<Post[]>;
  public posts: Observable<Post[]>;

  constructor(http: HttpClient, private router: Router) {
    super('posts', http);

    this.postsSubject = new BehaviorSubject<Post[]>({} as Post[]);
    this.posts = this.postsSubject.asObservable().pipe(distinctUntilChanged());

    this.type = DepartmentLevels.Mai;
    this.departmentLevel = PostType.All;
  }

  public get postsValue(): Post[] {
    return this.postsSubject.value;
  }

  getUserPosts(userId): Observable<Post[]> {
    return this.http.get<Post[]>(this.url + '/user/' + userId);
  }

  getUserFavPosts(userId): Observable<Post[]> {
    return this.http.get<Post[]>(this.url + '/user/' + userId + '/fav');
  }

  getNotifiedPosts() {
    return this.http.get<Post[]>(this.url + '/notified').subscribe(
      response =>  {
        this.postsSubject.next(response);
      });
  }

  searchPosts(terms: string) {
    if (terms === null || terms === '') {
      this.applyFilters();
      return;
    }

    return this.http.get<Post[]>(this.url + '/search/' + terms).pipe(debounceTime(10000)).subscribe(
      response =>  {
        if (JSON.stringify(response) === JSON.stringify(this.postsValue)) {
          return;
        }
        this.postsSubject.next(response);
      });
  }

  getPostComments(postId): Observable<Comment[]> {
    return this.http.get<Comment[]>(this.url + '/' + postId + '/comments');
  }

  applyFilters() {
    return this.http.get<Post[]>(this.url + `/filter/${this.type}/${this.departmentLevel}`).subscribe(
      response =>  {
        this.postsSubject.next(response);
        this.redirect();
      });
  }

  private redirect() {
    const type = PostType[this.type].toLowerCase();
    const departmentLevel = PostType[this.departmentLevel].toLowerCase();

    this.router.navigate(['/', type, departmentLevel]);
  }

  addCommentOnPost(postId, comment): Observable<Comment> {
    return this.http.post<Comment>(this.url + '/' + postId + '/addComment', JSON.stringify(comment));
  }



  delete(id): Observable<any> {
    return of(super.delete(id).subscribe(res => {
      const posts = this.postsValue.filter(post => post.id !== id);
      this.postsSubject.next(posts);
    }));
  }
}
