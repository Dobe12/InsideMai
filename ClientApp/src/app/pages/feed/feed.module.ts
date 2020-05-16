import {NgModule, SecurityContext} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FeedComponent} from "./feed.component";
import {MarkdownModule} from "ngx-markdown";
import {SharedModule} from "../../shared/shared.module";



@NgModule({
  declarations: [
    FeedComponent,

  ],
  imports: [
    MarkdownModule.forRoot({
      sanitize: SecurityContext.NONE
    }),
    SharedModule
  ]
})
export class FeedModule { }
