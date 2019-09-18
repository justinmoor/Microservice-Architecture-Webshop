import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product-service/product.service';
import { Product } from '../../models/product';

@Component({
    templateUrl: 'main.page.html',
    styleUrls: ['main.page.scss']
})
export class MainPage implements OnInit {
    producten: Product[] = [];
    message: string;

    constructor(private ps: ProductService) {

    }

    ngOnInit() {
        if (sessionStorage.getItem('message')) {
            this.message = sessionStorage.getItem('message');
            sessionStorage.removeItem('message');
        }
        this.ps.getProducts().subscribe(product => this.producten.push(product));
    }

}
