import { Injectable } from '@angular/core';
import { WinkelmandItem } from 'src/app/models/winkelmand-item';
import { Product } from '../../models/product';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WinkelmandService {

  items: WinkelmandItem [];

  private aantalSubject: BehaviorSubject<number>;

  constructor() {
    this.getProducts();
  }

  getProducts() {
    this.items = JSON.parse(localStorage.getItem('winkelmand'));

    if (this.items == null) {
      this.items = [];
    }

    this.aantalSubject = new BehaviorSubject<number>(this.totaalAantal());
  }

  private totaalAantal(): number {
    let aantal = 0;
    for (const item of this.items) {
      aantal += item.aantal;
    }
    return aantal;
  }

  aantal(): Observable<number> {
    return this.aantalSubject.asObservable();
  }

  addProduct(product: Product) {

    for (const item of this.items) {
      if (item.artikelnummer === product.artikelnummer) {
        item.aantal++;
        console.log(JSON.stringify(item));
        localStorage.setItem('winkelmand', JSON.stringify(this.items));
        return;
      }
    }

    this.items.push(new WinkelmandItem(product.artikelnummer, product.afbeeldingUrl, product.naam, product.prijs, product.prijsWithBtw, 1));
    localStorage.setItem('winkelmand', JSON.stringify(this.items));
    this.aantalSubject.next(this.totaalAantal());
  }

  addItem(currentItem: WinkelmandItem) {
    for (const item of this.items) {
      if (item.artikelnummer === currentItem.artikelnummer) {
        item.aantal++;
        localStorage.setItem('winkelmand', JSON.stringify(this.items));
        this.aantalSubject.next(this.totaalAantal());
        return;
      }
    }
  }

  removeItem(currentItem: WinkelmandItem) {
    for (const item of this.items) {
      if (item.artikelnummer === currentItem.artikelnummer) {
        if (item.aantal > 1) {
          item.aantal--;
          localStorage.setItem('winkelmand', JSON.stringify(this.items));
          this.aantalSubject.next(this.totaalAantal());
          return;
        }
      }
    }
  }

  removeProduct(currentItem: WinkelmandItem) {
    for (const item of this.items) {
      if (item.artikelnummer === currentItem.artikelnummer) {
          const index = this.items.indexOf(item);
          this.items.splice(index, 1);
          localStorage.setItem('winkelmand', JSON.stringify(this.items));
          this.aantalSubject.next(this.totaalAantal() - item.aantal);
          return;
      }
    }
  }

  resetWinkelmand() {
    this.aantalSubject.next(0);
    this.items = [];
    sessionStorage.removeItem('besteld');
    localStorage.removeItem('winkelmand');
  }

}
