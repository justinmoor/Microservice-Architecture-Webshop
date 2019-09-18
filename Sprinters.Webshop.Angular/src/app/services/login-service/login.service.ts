import { Injectable } from '@angular/core';
import { JwtService } from '../jwt-service/jwt.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Credentials } from 'src/app/models/credentials';
import { tap } from 'rxjs/operators';
import { JwtResult } from 'src/app/models/JwtResult';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient, private jwt: JwtService) { }

  logIn(credentials: Credentials): Observable<JwtResult> {
    return this.http.post<JwtResult>('/api/authenticatie/login', credentials)
      .pipe(
        tap(jwtResult => {
          this.jwt.token = jwtResult.token;
        })
      );
  }
}
