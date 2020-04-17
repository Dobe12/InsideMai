import { Component, OnInit } from '@angular/core';
import {PostsService} from "../../core/services/posts.service";

@Component({
  selector: 'app-search-form',
  templateUrl: './search-form.component.html',
  styleUrls: ['./search-form.component.scss']
})
export class SearchFormComponent implements OnInit {

  constructor(private postsService: PostsService) { }

  ngOnInit(): void {
  }

  searchPosts(terms: string) {
    this.postsService.searchPosts(terms);
  }
}
