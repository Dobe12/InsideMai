import {Component, Input, OnInit} from '@angular/core';
import {PostsService} from "../../core/services/posts.service";
import {DepartmentLevels, Post, PostType} from "../../core/models/post";
import {AuthService} from "../../core/auth/auth.service";
import {ActivatedRoute} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {transition, trigger, useAnimation} from "@angular/animations";
import {bounceIn} from "ng-animate";

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss'],
  animations: [
    trigger('bounceIn', [transition("void => *", useAnimation(bounceIn, {
      params: {
        delay: 0,
        timing: 0.3
      }
    }))])
  ]
})
export class FeedComponent implements OnInit {

  @Input() posts: Post[];
  DepartmentLevels = DepartmentLevels;
  isAnimate = false;

  constructor(public postsService: PostsService,
              private authService: AuthService,
              private route: ActivatedRoute,
              private toast: ToastrService) { }

  ngOnInit(): void {
    this.postsService.type = DepartmentLevels.Mai;
    this.postsService.departmentLevel = PostType.All;
    this.postsService.applyFilters();

    this.postsService.posts.subscribe(res => {
      this.posts = res;
    });
  }

  postsFilter() {
    this.postsService.getPostsByType(PostType.Advert).subscribe(res => {
      this.posts = res as Post[];
    });
  }

  setFilter(filter: DepartmentLevels) {
    this.postsService.departmentLevel = filter;
    this.postsService.applyFilters();
  }

  get departmentLevel() {
    return DepartmentLevels[this.postsService.departmentLevel].toLowerCase();
  }

  deletePost(id: number) {
    this.postsService.delete(id).subscribe(() => {
      this.posts = this.posts.filter(p => p.id !== id);
      this.toast.success("Пост успешно удален");
    });
  }
}
