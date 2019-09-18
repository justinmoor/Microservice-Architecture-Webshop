import { Injectable } from '@angular/core';
import { Klant } from '../../models/Klant';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RegistratieService {

  constructor(private http: HttpClient) { }

  registreer(klant: Klant) {
    return this.http.post('api/authenticatie/registreer', klant);
  }
}
