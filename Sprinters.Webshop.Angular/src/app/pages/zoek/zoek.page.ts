import { OnInit, Component } from '@angular/core';
import { ProductService } from 'src/app/services/product-service/product.service';
import { Product } from 'src/app/models/product';

@Component({
  templateUrl: './zoek.page.html',
  styleUrls: ['./zoek.page.scss']
})

export class ZoekPage {

  constructor(private ps: ProductService) { }

  searchProducten(): Product[] {
    return this.ps.searchProducts;
  }
}
