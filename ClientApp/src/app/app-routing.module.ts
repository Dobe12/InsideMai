import { NgModule } from '@angular/core';
import {Routes, RouterModule, PreloadAllModules} from '@angular/router';
import {LoginComponent} from "./pages/login/login.component";
import {LoginFormComponent} from "./components/login-form/login-form.component";
import {RestorePasswordFormComponent} from "./components/restore-password-form/restore-password-form.component";
import {MainComponent} from "./shared/layout/main/main.component";
import {AuthGuard} from "./core/auth/auth-guard.service";
import {FeedComponent} from "./pages/feed/feed.component";
import {UserPostComponent} from "./pages/user-post/user-post.component";
import {UserProfileComponent} from "./pages/user-profile/user-profile.component";
import {CreatePostComponent} from "./pages/create-post/create-post.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";


const routes: Routes = [
  {path: 'login', loadChildren: () => import(`./pages/login/login.module`).then(m => m.LoginModule)},
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [
      {path: '', component: FeedComponent},
      {path: 'post/:id', component: UserPostComponent},
      {path: 'user/:id', component: UserProfileComponent},
      {path: ':type/:level', component: FeedComponent },
      {path: 'create', component: CreatePostComponent},
      {path: '**', component: NotFoundComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
