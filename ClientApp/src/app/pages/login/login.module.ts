import { NgModule } from '@angular/core';
import {LoginComponent} from "./login.component";
import {LoginFormComponent} from "../../components/login-form/login-form.component";
import {RestorePasswordFormComponent} from "../../components/restore-password-form/restore-password-form.component";
import {SharedModule} from "../../shared/shared.module";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [
    LoginComponent,
    LoginFormComponent,
    RestorePasswordFormComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: LoginComponent,
        children : [
          {path: '', component: LoginFormComponent},
          {path: 'restore', component: RestorePasswordFormComponent}
        ]},
    ])
  ],
})
export class LoginModule { }
