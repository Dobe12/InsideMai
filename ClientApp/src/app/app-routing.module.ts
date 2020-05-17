import { NgModule } from '@angular/core';
import {Routes, RouterModule, PreloadAllModules} from '@angular/router';
import {MainComponent} from "./shared/layout/main/main.component";
import {AuthGuard} from "./core/auth/auth-guard.service";
import {NotFoundComponent} from "./pages/not-found/not-found.component";


const routes: Routes = [
  {path: 'login',
    loadChildren: () => import(`./pages/login/login.module`)
      .then(m => m.LoginModule)},
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [
      {path: 'post/:id',
        loadChildren: () => import(`./pages/user-post/user-post.module`)
          .then(m => m.UserPostModule)},
      {path: 'user/:id',
        loadChildren: () => import(`./pages/user-profile/user-profile.module`)
          .then(m => m.UserProfileModule)},
      {path: 'create',
        loadChildren: () => import(`./pages/create-post/create-post.module`)
        .then(m => m.CreatePostModule)},
      {path: '',
        loadChildren: () => import(`./pages/feed/feed.module`)
          .then(m => m.FeedModule)},
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
