import { Component, OnInit } from '@angular/core';
import {PostsService} from "../../../core/services/posts.service";
import {PostType} from "../../../core/models/post";

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {
  constructor(private postsService: PostsService) { }
  PostType = PostType;
  ngOnInit(): void {
  }

  setFilter(filter: PostType) {
    this.postsService.type = filter;
    this.postsService.applyFilters();
  }

  get postType() {
    return PostType[this.postsService.type].toLowerCase();
  }

}
