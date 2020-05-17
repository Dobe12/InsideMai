import { NgModule } from '@angular/core';
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {HttpErrorInterceptor} from "./interceptros/http.interceptor";
import {JwtInterceptor} from "./interceptros/jwt.interceptor";
import {PostsService} from "./services/posts.service";
import {CommentsService} from "./services/comments.service";
import {DepartmentsService} from "./services/deparments.service";
import {UsersService} from "./services/users.service";
import {JwtHelperService, JwtModule} from "@auth0/angular-jwt";
import {AuthGuard} from "./auth/auth-guard.service";
import {SharedModule} from "../shared/shared.module";


@NgModule({
  declarations: [],
  imports: [
    SharedModule,
    JwtModule,

  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    PostsService,
    CommentsService,
    DepartmentsService,
    UsersService,
    JwtHelperService,
    AuthGuard
  ],
})
export class CoreModule { }
