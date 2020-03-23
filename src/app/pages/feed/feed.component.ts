import { Component, OnInit, Input } from '@angular/core';
import {PostsService} from "../../core/services/posts.service";
import {Post} from "../../core/models/post";

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {
  @Input() posts: Post[];
  constructor(private postsSerice: PostsService) { }

  ngOnInit(): void {
    this.postsSerice.getAll().subscribe(posts => {
      this.posts = posts as Post[];
    });
  }

}
