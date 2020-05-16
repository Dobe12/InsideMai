import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, FormControlName, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../core/auth/auth.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-remember-password-form',
  templateUrl: './restore-password-form.component.html',
  styleUrls: ['./restore-password-form.component.scss']
})
export class RestorePasswordFormComponent implements OnInit {

  phone: FormControl;
  phoneIsInvalid = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private toastr: ToastrService) {
    this.phone = this.fb.control('+7', [Validators.required, Validators.pattern("[+0-9 ]{12}")]);
  }

  ngOnInit(): void {}

  restore() {
    if (this.phone.valid) {
      this.phoneIsInvalid = false;
      console.log('Отправляем пароль на телефон....');
      this.toastr.success("Новый пароль отправлен на телефон");
      this.router.navigateByUrl('/login');
    } else {
      this.phoneIsInvalid = true;
    }
  }
}

