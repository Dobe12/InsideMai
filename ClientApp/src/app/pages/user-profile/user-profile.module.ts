import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {UserProfileComponent} from "./user-profile.component";
import {ChangePasswordFormComponent} from "../../components/change-password-form/change-password-form.component";
import {ConfirmDeleteModalComponent} from "../../components/confirm-delete-modal/confirm-delete-modal.component";
import {SharedModule} from "../../shared/shared.module";



@NgModule({
  declarations: [
    UserProfileComponent,
    ChangePasswordFormComponent,
  ],
  imports: [
    SharedModule
  ],
  entryComponents: [ChangePasswordFormComponent]
})
export class UserProfileModule { }
