import { Injectable, OnDestroy } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable, Subscriber } from 'rxjs';
import { Product } from '../../models/product';


@Injectable({
  providedIn: 'root'
})
export class ProductService implements OnDestroy {
  searchProducts: Product[];
  producten: Product[];
  subscriber: Subscriber<Product>;
  observer: Observable<Product>;

  constructor(private http: HttpClient) {
    this.observer = new Observable<Product>(subscriber => {
      this.subscriber = subscriber;
    });

  }

  getProducts(): Observable<Product> {
    this.http.get<Product[]>('/api/artikelen').subscribe(
      data => {
        this.producten = data;
        data.forEach(b => this.subscriber.next(b));
      }
    );

    return this.observer;
  }

  filterProducts(filter: String): Product[] {
    this.searchProducts = [];
    for (let item of this.producten) {
      if (item['naam'].toString().toLowerCase().includes(filter.toLocaleLowerCase())
        ||
        item['beschrijving'].toString().toLowerCase().includes(filter.toLocaleLowerCase())) {
        this.searchProducts.push(item);
      }
    }
    return this.searchProducts;
  }

  getProduct(id: number) {
    return this.http.get<Product>('api/artikelen/' + id);
  }

  ngOnDestroy() {
    this.subscriber.unsubscribe();
  }
}
