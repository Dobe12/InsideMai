import { NgModule } from '@angular/core';
import {CommentComponent} from "../../components/comment/comment.component";
import {NewCommentFormComponent} from "../../components/new-comment-form/new-comment-form.component";
import {UserPostComponent} from "./user-post.component";
import {SharedModule} from "../../shared/shared.module";
import {RouterModule} from "@angular/router";


@NgModule({
  declarations: [
    UserPostComponent,
    CommentComponent,
    NewCommentFormComponent,

  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: UserPostComponent},
    ])
  ]
})
export class UserPostModule { }
