import {NgModule, SecurityContext} from '@angular/core';
import {FeedComponent} from "./feed.component";
import {SharedModule} from "../../shared/shared.module";
import {RouterModule} from "@angular/router";




@NgModule({
  declarations: [
    FeedComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: ':type/:level', component: FeedComponent },
      {path: '', component: FeedComponent}
    ])
  ]
})
export class FeedModule { }
