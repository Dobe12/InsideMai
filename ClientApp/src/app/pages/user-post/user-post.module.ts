import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {CommentComponent} from "../../components/comment/comment.component";
import {NewCommentFormComponent} from "../../components/new-comment-form/new-comment-form.component";
import {UserPostComponent} from "./user-post.component";
import {SharedModule} from "../../shared/shared.module";



@NgModule({
  declarations: [
    UserPostComponent,
    CommentComponent,
    NewCommentFormComponent,

  ],
  imports: [
    SharedModule
  ]
})
export class UserPostModule { }
