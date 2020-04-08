import { Component, OnInit, Input } from '@angular/core';
import {PostsService} from "../../core/services/posts.service";
import {Post} from "../../core/models/post";
import {AuthService} from "../../core/auth/auth.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  @Input() posts: Post[];
  constructor(private postsService: PostsService,
              private authService: AuthService) { }

  ngOnInit(): void {
    this.getAllPosts();
  }

  getAllPosts() {
    this.postsService.getAll().subscribe(posts => {
      this.posts = posts as Post[];
    });
  }

  getUniversityPosts() {
    this.getPostsByLvl(environment.UniversityLvl);
  }

  getDepartmentPosts() {
    this.getPostsByLvl(environment.DepartmentLvl);
  }

  getGroupPosts() {
    this.getPostsByLvl(environment.GroupLvl);
  }

  private getPostsByLvl(lvl) {
    this.postsService.getPostsDepartmentByLevel(this.authService.currentUserValue.department.id, lvl)
      .subscribe(result => {
        this.posts = result as Post[];
      });
}


}
