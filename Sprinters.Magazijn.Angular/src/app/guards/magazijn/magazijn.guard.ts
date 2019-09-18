import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { JwtService } from '../../services/jwt-service/jwt.service';

@Injectable({
  providedIn: 'root'
})
export class MagazijnGuard implements CanActivate {
  
  constructor(private jwtService: JwtService, private router: Router){ }

  canActivate() {
    if(this.jwtService.isMagazijn()) return true;

    this.router.navigate(["/beheer"])
    return false;
  }
}

  