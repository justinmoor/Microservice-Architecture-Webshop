import { Injectable, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscriber } from 'rxjs';
import { Bestelling } from 'src/app/models/Bestelling';

@Injectable({
  providedIn: 'root'
})
export class BestellingService {
  subscriber: Subscriber<any>;

  constructor(private http: HttpClient) { }

  bestellingAfkeuren(id: number): any {
    return this.http.post<Bestelling[]>('/api/bestellingen/sales/afkeuren/' + id, id).subscribe(bestellingen => {
      bestellingen.forEach(id => {
        this.subscriber.next(id)
      });
      this.subscriber.next(id);
    }, error => {
      this.subscriber.error(error)
    });
  }

  bestellingGoedKeuren(id: number): any {
    return this.http.post<Bestelling>('/api/bestellingen/sales/goedkeuren/' + id, id).subscribe(id => {
      this.subscriber.next(id)
    }, error => {
      this.subscriber.error(error)
    });
  }

  getBestellingenBoven500Euro(): any {
    let observer = new Observable<Bestelling>(subscriber => {
      this.subscriber = subscriber;
    })

    this.http.get<Bestelling[]>('/api/bestellingen/sales').subscribe(
      bestelling => {
        bestelling.forEach(b => this.subscriber.next(b));
      }
      , error => { this.subscriber.error(error) });

    return observer;
  }
}