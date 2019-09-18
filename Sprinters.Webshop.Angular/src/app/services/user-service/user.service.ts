import { Injectable } from '@angular/core';
import { JwtService } from '../jwt-service/jwt.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Klant } from '../../models/Klant';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private jwtService: JwtService) { }

  getKlantGegevens(): Observable<Klant> {
    return this.http.get<Klant>('/api/klanten/' + this.jwtService.getTokenUserId());
  }

}
