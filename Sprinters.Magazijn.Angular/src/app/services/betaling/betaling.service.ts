import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Betaling } from '../../models/betaling';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
  })
  export class BetalingService{
    constructor(private http: HttpClient){
        
    }

    plaats(betaling: Betaling) {
        return this.http.post('/api/betalingen', betaling);
    }
  }