import { BrowserModule } from '@angular/platform-browser';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import {ErrorHandler, NgModule, SecurityContext} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule, HttpInterceptor} from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditorModule } from '@tinymce/tinymce-angular';

import { ToastrModule } from 'ngx-toastr';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/layout/header/header.component';
import { SearchFormComponent } from './components/search-form/search-form.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MainComponent } from './shared/layout/main/main.component';
import { SidenavComponent } from './shared/layout/sidenav/sidenav.component';
import { FeedComponent } from './pages/feed/feed.component';
import { PostComponent } from './components/post/post.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';
import { CommentComponent } from './components/comment/comment.component';
import { UserPostComponent } from './pages/user-post/user-post.component';
import { LikeComponent } from './components/like/like.component';
import { NewCommentFormComponent } from './components/new-comment-form/new-comment-form.component';
import {HttpErrorInterceptor} from "./core/interceptros/http.interceptor";
import {PostsService} from "./core/services/posts.service";
import {CommentsService} from "./core/services/comments.service";
import {DeparmentsService} from "./core/services/deparments.service";
import {UsersService} from "./core/services/users.service";
import {RouterModule} from "@angular/router";
import { LoginComponent } from './pages/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {JwtHelperService, JwtModule} from "@auth0/angular-jwt";
import {AuthGuard} from "./core/auth/auth-guard.service";
import {JwtInterceptor} from "./core/interceptros/jwt.interceptor";
import { CreatePostComponent } from './pages/create-post/create-post.component';
import {MatInputModule} from "@angular/material/input";
import {MarkdownModule} from "ngx-markdown";
import {MatCheckboxModule} from "@angular/material/checkbox";

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SearchFormComponent,
    ProfileComponent,
    MainComponent,
    SidenavComponent,
    FeedComponent,
    PostComponent,
    UserProfileComponent,
    CommentComponent,
    UserPostComponent,
    LikeComponent,
    NewCommentFormComponent,
    LoginComponent,
    CreatePostComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TextareaAutosizeModule,
    HttpClientModule,
    BrowserAnimationsModule,
    EditorModule,
    MarkdownModule.forRoot({
      sanitize: SecurityContext.NONE
    }),
    ToastrModule.forRoot({
      timeOut: 1500
    }),
    RouterModule.forRoot([
      {path: 'login', component: LoginComponent},
      {
        path: '',
        component: MainComponent,
        canActivate: [AuthGuard],
        children: [
          {path: '', component: FeedComponent},
          {path: 'post/:id', component: UserPostComponent},
          {path: 'user/:id', component: UserProfileComponent},
          {path: ':type/:level', component: FeedComponent },
          {path: 'create', component: CreatePostComponent}
        ]
      }
    ]),
    ReactiveFormsModule,
    FormsModule,
    JwtModule,
    MatInputModule,
    MatCheckboxModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    PostsService,
    CommentsService,
    DeparmentsService,
    UsersService,
    JwtHelperService,
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
