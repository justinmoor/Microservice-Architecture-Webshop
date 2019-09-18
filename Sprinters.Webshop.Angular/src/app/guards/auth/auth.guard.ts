import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtService } from 'src/app/services/jwt-service/jwt.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private jwtService: JwtService, private router: Router) {}

  canActivate() {
    if (this.jwtService.isLoggedIn()) {
      return true;
    }
    this.router.navigate(['/inloggen']);
    return false;
  }
}
