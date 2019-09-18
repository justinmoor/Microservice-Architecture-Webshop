import { Component, OnInit, Input } from '@angular/core';
import { Product } from '../../models/product';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit {
  @Input() product: Product;
  bevestiging: boolean;

  constructor(private winkelmandService: WinkelmandService) {

  }

  ngOnInit() {

  }

  addProduct() {
    this.winkelmandService.addProduct(this.product);
    this.bevestiging = true;
    setTimeout(() => {
      this.bevestiging = false;
    }, 2500);
  }
}
