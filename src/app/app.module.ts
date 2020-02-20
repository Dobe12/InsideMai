import { BrowserModule } from '@angular/platform-browser';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layouts/header/header.component';
import { SearchFormComponent } from './components/search-form/search-form.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MainComponent } from './layouts/main/main.component';
import { SidenavComponent } from './layouts/main/sidenav/sidenav.component';
import { FeedComponent } from './layouts/main/feed/feed.component';
import { PostComponent } from './components/post/post.component';
import { UserProfileComponent } from './layouts/main/user-profile/user-profile.component';
import { CommentComponent } from './components/comment/comment.component';
import { UserPostComponent } from './layouts/main/user-post/user-post.component';
import { LikeComponent } from './components/like/like.component';
import { NewCommentFormComponent } from './components/new-comment-form/new-comment-form.component';

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
    NewCommentFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TextareaAutosizeModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
