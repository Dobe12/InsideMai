import { BrowserModule } from '@angular/platform-browser';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import {ErrorHandler, NgModule, SecurityContext} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule, HttpInterceptor} from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { EditorModule } from '@tinymce/tinymce-angular';
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { ToastrModule } from 'ngx-toastr';
import {MatInputModule} from "@angular/material/input";
import {MarkdownModule} from "ngx-markdown";
import {MatCheckboxModule} from "@angular/material/checkbox";


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
import {DepartmentsService} from "./core/services/deparments.service";
import {UsersService} from "./core/services/users.service";
import {RouterModule} from "@angular/router";
import { LoginComponent } from './pages/login/login.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {JwtHelperService, JwtModule} from "@auth0/angular-jwt";
import {AuthGuard} from "./core/auth/auth-guard.service";
import {JwtInterceptor} from "./core/interceptros/jwt.interceptor";
import { CreatePostComponent } from './pages/create-post/create-post.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RestorePasswordFormComponent } from './components/restore-password-form/restore-password-form.component';
import { ChangePasswordFormComponent } from './components/change-password-form/change-password-form.component';
import { ConfirmDeleteModalComponent } from './components/confirm-delete-modal/confirm-delete-modal.component';
import {CoreModule} from "./core/core.module";
import {SharedModule} from "./shared/shared.module";
import {CreatePostModule} from "./pages/create-post/create-post.module";
import {FeedModule} from "./pages/feed/feed.module";
import {LoginModule} from "./pages/login/login.module";
import {UserPostModule} from "./pages/user-post/user-post.module";
import {UserProfileModule} from "./pages/user-profile/user-profile.module";

@NgModule({
  declarations: [
    AppComponent,
    SidenavComponent,
    HeaderComponent,
    MainComponent,
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    CoreModule,
    SharedModule,
    CreatePostModule,
    FeedModule,
    LoginModule,
    UserPostModule,
    UserProfileModule,
    AppRoutingModule
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
