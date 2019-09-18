import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LoginService } from 'src/app/services/login-service/login.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
  loginForm: FormGroup;
  error = false;
  message: string;
  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private router: Router,
  ) { }

  ngOnInit() {
    if (sessionStorage.getItem('message')) {
      this.message = sessionStorage.getItem('message');
      sessionStorage.removeItem('message');
    }
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  isFieldInvalid(field: string) {
      return this.loginForm.get(field).touched && this.loginForm.get(field).invalid;
  }

  login() {
    this.loginService.logIn(this.loginForm.value).subscribe(
      () => {
        sessionStorage.setItem('message', 'Je bent succesvol ingelogd');
        this.router.navigate(['/']);
      },
    () => {
        this.error = true;
      });

  }
}
