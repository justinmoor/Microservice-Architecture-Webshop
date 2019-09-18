import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Bestelling } from '../../models/bestelling';

@Injectable({
  providedIn: 'root'
})
export class BestelService {

  constructor(private http: HttpClient) {

  }

  plaatsBestelling(bestelling: Bestelling) {
   return this.http.post('/api/bestellingen', bestelling);
  }

}
