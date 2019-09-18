import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BesteldGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate() {
    const besteld = JSON.parse(sessionStorage.getItem('besteld'));

    if (besteld !== null) {
      return true;
    }

    this.router.navigate(['/']);
    return false;
  }
}
