import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, ParamMap} from '@angular/router';
import {UsersService} from "../../core/services/users.service";
import {User} from "../../core/models/user";
import {Post} from "../../core/models/post";
import {PostsService} from "../../core/services/posts.service";
import {combineLatest, forkJoin, Observable, of} from "rxjs";
import {mergeMap} from "rxjs/operators";

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user: User;
  userPosts: Post[];

  constructor(private route: ActivatedRoute,
              private usersService: UsersService,
              private postsService: PostsService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(mergeMap((param: ParamMap) => {
      const id = param.get('id');

      const user = this.usersService.get(id);
      const userPosts = this.postsService.getUserPost(id);

      return forkJoin([user, userPosts]);
    })).subscribe(result => {
      this.user = result[0] as User;
      this.userPosts = result[1] as Post[];
    });
  }
}
