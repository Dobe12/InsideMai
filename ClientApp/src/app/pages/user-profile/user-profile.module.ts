import { NgModule } from '@angular/core';
import {UserProfileComponent} from "./user-profile.component";
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";
import {SharedModule} from "../../shared/shared.module";
import {RouterModule} from "@angular/router";



@NgModule({
  declarations: [
    UserProfileComponent,
    ChangePasswordFormComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: UserProfileComponent},
    ])
  ],
  entryComponents: [ChangePasswordFormComponent]
})
export class UserProfileModule { }
