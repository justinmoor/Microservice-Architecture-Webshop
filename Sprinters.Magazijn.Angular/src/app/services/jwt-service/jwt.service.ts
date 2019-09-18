import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

export const ACCESS_TOKEN_KEY = 'access_token';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  getTokenUserId(): any {
    const decoded = this.decodedToken();

    return decoded.UserId;
  }

  set token(value: string) {
    window.localStorage.setItem(ACCESS_TOKEN_KEY, value);
  }

  get token(): string {
    return window.localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  decodedToken() {
    return new JwtHelperService().decodeToken(this.token);
  }

  getTokenExpirationDate(): Date {
    const decoded = this.decodedToken();

    if (decoded.exp === undefined) return null;

    const date = new Date(0); 
    date.setUTCSeconds(decoded.exp);
    return date;
  }

  logout(){
    this.token = null;
    window.localStorage.removeItem(ACCESS_TOKEN_KEY);
  }

  isLoggedIn(){
    if(!this.token) return false;

    const date = this.getTokenExpirationDate();

    if(date === undefined) return false;
    
    if(!(date.valueOf() > new Date().valueOf())){
      this.logout();
      return false;
    }
    return true;
  }

  isMagazijn(){
    if(!this.isLoggedIn()) return false;

    return this.isRole(this.decodedToken(), "Magazijn");
  }

  isSales(){
    if(!this.isLoggedIn()) return false;

    return this.isRole(this.decodedToken(), "Sales");
  }

  isKlant(){
    if(!this.isLoggedIn()) return false;

    return this.isRole(this.decodedToken(), "Klant");
  }

  private isRole(decoded: string, role: string): boolean{
    if(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === undefined) return false;

    if(decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] == role) return true;
  }
}
