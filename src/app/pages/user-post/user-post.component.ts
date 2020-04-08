import { Component, OnInit } from '@angular/core';
import {PostsService} from "../../core/services/posts.service";
import {Post} from "../../core/models/post";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {mergeMap} from "rxjs/operators";
import {forkJoin} from "rxjs";
import {Comment} from "../../core/models/comment";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-user-post',
  templateUrl: './user-post.component.html',
  styleUrls: ['./user-post.component.scss']
})

export class UserPostComponent implements OnInit {
  post: Post;
  postComments: Comment[];
  constructor(private postsService: PostsService,
              private route: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.route.paramMap.pipe(mergeMap((param: ParamMap) => {
      const id = param.get('id');

      const post = this.postsService.get(id);
      const postComments = this.postsService.getPostComments(id);

      return forkJoin([post, postComments]);
    })).subscribe(result => {
      this.post = result[0] as Post;
      this.postComments = result[1] as Comment[];
      console.log(this.postComments);

    });

  }

  addComment(comment: Comment) {
    this.postsService.addCommentOnPost(this.post.id, comment).subscribe(res => {
      this.postComments.push(res as Comment);
      this.post.commentsCount++;
      this.toastr.success("Комментарий добавлен");
    });
  }


}
