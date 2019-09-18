import { Injectable } from '@angular/core';
import {  Observable, BehaviorSubject } from 'rxjs';
import { Bestelling } from '../../models/bestelling';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class InpakService {
  geenNieuweBestelling: boolean = false;
  constructor(private http: HttpClient, private router: Router) { }

  getVolgendeBestelling(): Observable<Bestelling> {
    return this.http.get<Bestelling>('/api/bestellingen/next');
  }

  rondBestellingAf(id: number) {
    this.http.post<number>('/api/bestellingen/finish/' + id, id).subscribe();
  }
}
