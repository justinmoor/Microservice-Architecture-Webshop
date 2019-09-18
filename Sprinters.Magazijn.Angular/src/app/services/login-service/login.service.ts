import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Credentials } from 'src/app/models/credentials';
import { tap } from 'rxjs/operators';
import { JwtResult } from 'src/app/models/JwtResult';
import { JwtService } from '../jwt-service/jwt.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient, private jwt: JwtService, private router: Router) { }

  logIn(credentials: Credentials): Observable<JwtResult> {
    return this.http.post<JwtResult>('/api/authenticatie/login', credentials)
      .pipe(
        tap(jwtResult => {
          this.jwt.token = jwtResult.token;
        })
      );
  }

  logout() {
    this.jwt.logout();
    this.router.navigate(["/inloggen"]);
  }
}
