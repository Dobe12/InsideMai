import { Component, OnInit } from '@angular/core';
import {AuthService} from "../../core/auth/auth.service";
import {Router} from "@angular/router";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  submitted = false;
  authForm: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {
    this.authForm = this.fb.group(
      {
        email: ['', Validators.required],
        password: ['', Validators.required]
      });
  }

  ngOnInit(): void {}

  signIn() {
    this.submitted = true;

    const credentials = this.authForm.value;
    this.authService.login(credentials)
      .subscribe(result => {
        if (result) {
          this.toastr.success('Вы успешно вошли!');
          this.router.navigate(['/']);
        }
      });
  }

  get email() { return this.authForm.get('email'); }
  get password() { return this.authForm.get('password'); }

}
