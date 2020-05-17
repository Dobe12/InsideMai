import {NgModule, SecurityContext} from '@angular/core';
import { CommonModule } from '@angular/common';
import {SearchFormComponent} from "../components/search-form/search-form.component";
import {PostComponent} from "../components/post/post.component";
import {LikeComponent} from "../components/like/like.component";
import {NotFoundComponent} from "../pages/not-found/not-found.component";
import {ConfirmDeleteModalComponent} from "../components/confirm-delete-modal/confirm-delete-modal.component";
import {ProfileComponent} from "../components/profile/profile.component";
import {TextareaAutosizeModule} from "ngx-textarea-autosize";
import {HttpClientModule} from "@angular/common/http";
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {ToastrModule} from "ngx-toastr";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {RouterModule} from "@angular/router";
import {MarkdownModule} from "ngx-markdown";



@NgModule({
  declarations: [
    SearchFormComponent,
    PostComponent,
    LikeComponent,
    NotFoundComponent,
    ConfirmDeleteModalComponent,
    ProfileComponent,

  ],
  imports: [
    CommonModule,
    TextareaAutosizeModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatCheckboxModule,
    RouterModule,
    MarkdownModule.forRoot({
      sanitize: SecurityContext.NONE
    }),
    ToastrModule.forRoot({
      timeOut: 1500
    }),
  ],
  exports: [
    CommonModule,
    SearchFormComponent,
    PostComponent,
    LikeComponent,
    RouterModule,
    NotFoundComponent,
    ConfirmDeleteModalComponent,
    ProfileComponent,
    TextareaAutosizeModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatCheckboxModule,
    MarkdownModule,
    ToastrModule
  ],
  entryComponents: [ConfirmDeleteModalComponent]
})
export class SharedModule { }
