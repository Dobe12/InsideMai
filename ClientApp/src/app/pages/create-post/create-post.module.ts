import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NewCommentFormComponent} from "../../components/new-comment-form/new-comment-form.component";
import {CreatePostComponent} from "./create-post.component";
import {EditorModule} from "@tinymce/tinymce-angular";
import {SharedModule} from "../../shared/shared.module";



@NgModule({
  declarations: [
    CreatePostComponent,

  ],
  imports: [
    SharedModule,
    EditorModule,
  ]
})
export class CreatePostModule { }
