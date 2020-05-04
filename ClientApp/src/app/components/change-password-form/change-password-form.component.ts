import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../core/auth/auth.service";
import {ToastrService} from "ngx-toastr";
import {Router} from "@angular/router";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-change-password-form',
  templateUrl: './change-password-form.component.html',
  styleUrls: ['./change-password-form.component.scss']
})
export class ChangePasswordFormComponent implements OnInit {

  newPasswordForm: FormGroup;
  wrongPassword = false;
  constructor(private fb: FormBuilder,
              private auth: AuthService,
              private toastr: ToastrService,
              private router: Router,
              public dialogRef: MatDialogRef<ChangePasswordFormComponent>) {
    this.newPasswordForm = fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      repeatNewPassword: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

get currentPassword() {
    return this.newPasswordForm.get('currentPassword');
}

  get newPassword() {
    return this.newPasswordForm.get('newPassword');
  }

  get repeatNewPassword() {
    return this.newPasswordForm.get('repeatNewPassword');
  }

  onSubmit() {
    if (this.newPasswordForm.invalid) {
      return;
    }

    this.auth.changePassword(this.currentPassword.value, this.newPassword.value).subscribe(res => {
      this.toastr.success("Пароль успешно изменен");
      this.wrongPassword = false;
      this.onCloseModal();
      this.newPasswordForm.reset();
    }, error => {
      if (error === 'Код ошибки: 400. Неверный пароль') {
        this.wrongPassword = true;
        this.currentPassword.reset();
      }
    });
  }

  onCloseModal() {
    this.dialogRef.close();
  }
}
