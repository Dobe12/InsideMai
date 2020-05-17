import { NgModule } from '@angular/core';
import {CreatePostComponent} from "./create-post.component";
import {EditorModule} from "@tinymce/tinymce-angular";
import {SharedModule} from "../../shared/shared.module";
import {RouterModule} from "@angular/router";




@NgModule({
  declarations: [
    CreatePostComponent,
  ],
  imports: [
    SharedModule,
    EditorModule,
    RouterModule.forChild([
      {path: '', component: CreatePostComponent},
    ])
  ]
})
export class CreatePostModule { }
