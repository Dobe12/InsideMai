import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {AuthService} from "../../../core/auth/auth.service";
import {User} from "../../../core/models/user";
import {PostsService} from "../../../core/services/posts.service";
import {Observable} from "rxjs";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  notificationCounter: number;
  currentUser$: Observable<User>;

  @Output() onShowNewPosts = new EventEmitter();

  constructor(public auth: AuthService,
              private postsService: PostsService) {

    this.currentUser$ = this.auth.currentUser;
  }

  ngOnInit(): void {

  }

  showNewPosts() {
    this.postsService.getNotifiedPosts();
    this.notificationCounter = 0;
  }
}
