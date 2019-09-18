import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/login-service/login.service';
import { UserService } from 'src/app/services/user-service/user.service';
import { JwtService } from '../../services/jwt-service/jwt.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})
export class LoginFormComponent implements OnInit {
  loginForm: FormGroup;
  error: boolean = false;

  constructor(
    private fb: FormBuilder,
    private loginService: LoginService,
    private router: Router,
    private jwtService: JwtService
  ) { }

  ngOnInit() {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  isFieldInvalid(field: string) {
    return this.loginForm.get(field).touched && this.loginForm.get(field).invalid;
  }

  login() {
    this.loginService.logIn(this.loginForm.value).subscribe(jwtResult => {

      if(this.jwtService.isKlant()){
        this.loginService.logout()
        return;
      }

      if(this.jwtService.isMagazijn()){
        this.router.navigate(["/magazijn-dashboard"]);
        return;
      }

      if(this.jwtService.isSales()){
        this.router.navigate(["/sales"]);
        return;
      }
    }, 
    error => {
      this.error = true;
    });
  }
}
