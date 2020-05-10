import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {AuthService} from "../../../core/auth/auth.service";
import {User} from "../../../core/models/user";
import {PostsService} from "../../../core/services/posts.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  notificationCounter: number;

  @Output() onShowNewPosts = new EventEmitter();

  constructor(public auth: AuthService,
              private postsService: PostsService) {
  }

  ngOnInit(): void {
    this.auth.currentUser.subscribe(res => this.notificationCounter = res.notificationsCount);
  }

  showNewPosts() {
    this.postsService.getNotifiedPosts();
    this.notificationCounter = 0;
  }
}
