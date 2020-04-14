import {Component, Input, OnInit} from '@angular/core';
import {PostsService} from "../../core/services/posts.service";
import {DepartmentLevels, Post, PostType} from "../../core/models/post";
import {AuthService} from "../../core/auth/auth.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  @Input() posts: Post[];
  DepartmentLevels = DepartmentLevels;
  constructor(public postsService: PostsService,
              private authService: AuthService,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.postsService.type = DepartmentLevels.Mai;
    this.postsService.departmentLevel = PostType.All;
    this.postsService.applyFilters();
  }

  postsFilter() {
    this.postsService.getPostsByType(PostType.Advert).subscribe(res => {
      this.posts = res as Post[];
    });
    console.log('wtf');
  }

  setFilter(filter: DepartmentLevels) {
    this.postsService.departmentLevel = filter;
    this.postsService.applyFilters();
  }

  get departmentLevel() {
    return DepartmentLevels[this.postsService.departmentLevel].toLowerCase();
  }

}
