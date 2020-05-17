import { BrowserModule } from '@angular/platform-browser';
import { TextareaAutosizeModule } from 'ngx-textarea-autosize';
import {NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClient, HttpClientModule, HttpInterceptor} from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/layout/header/header.component';
import { MainComponent } from './shared/layout/main/main.component';
import { SidenavComponent } from './shared/layout/sidenav/sidenav.component';
import {CoreModule} from "./core/core.module";
import {SharedModule} from "./shared/shared.module";


@NgModule({
  declarations: [
    AppComponent,
    SidenavComponent,
    HeaderComponent,
    MainComponent,
  ],
  imports: [
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
    BrowserModule,
    CoreModule,
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
