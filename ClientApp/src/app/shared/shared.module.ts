import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SearchFormComponent} from "../components/search-form/search-form.component";
import {MainComponent} from "./layout/main/main.component";
import {PostComponent} from "../components/post/post.component";
import {LikeComponent} from "../components/like/like.component";
import {NotFoundComponent} from "../pages/not-found/not-found.component";
import {ConfirmDeleteModalComponent} from "../components/confirm-delete-modal/confirm-delete-modal.component";
import {SidenavComponent} from "./layout/sidenav/sidenav.component";
import {HeaderComponent} from "./layout/header/header.component";
import {ProfileComponent} from "../components/profile/profile.component";
import {TextareaAutosizeModule} from "ngx-textarea-autosize";
import {HttpClientModule} from "@angular/common/http";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {ToastrModule} from "ngx-toastr";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {JwtModule} from "@auth0/angular-jwt";
import {MatInputModule} from "@angular/material/input";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {ChangePasswordFormComponent} from "../components/change-password-form/change-password-form.component";
import {AppRoutingModule} from "../app-routing.module";
import {RouterModule} from "@angular/router";



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
    ToastrModule
  ],
  entryComponents: [ConfirmDeleteModalComponent]
})
export class SharedModule { }
