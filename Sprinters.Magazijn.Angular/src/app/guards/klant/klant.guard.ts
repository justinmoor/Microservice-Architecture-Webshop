import { CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { JwtService } from '../../services/jwt-service/jwt.service';

@Injectable({
    providedIn: 'root'
  })
  export class KlantGuard implements CanActivate {
    
    constructor(private jwtService: JwtService, private router: Router){ }
  
    canActivate() {
      if(this.jwtService.isKlant() || !this.jwtService.isLoggedIn()){
          this.jwtService.logout();
          this.router.navigate(["/inloggen"]);  
          return false;
      }
     
      return true;
    }
  }