import { Component, OnInit, Input } from '@angular/core';
import {Post} from "../../core/models/post";

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {
@Input() post: Post;
  constructor() { }

  ngOnInit(): void {
    if (this.post.isAnonymous) {
      this.post.author.fullName = "Анонимно";
      this.post.author.userPic = 'assets/images/defaultUserPic.png';
    }
  }
}
