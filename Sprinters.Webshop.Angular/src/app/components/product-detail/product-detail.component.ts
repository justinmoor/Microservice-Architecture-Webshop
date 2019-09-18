import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product-service/product.service';
import { Product } from '../../models/product';
import { Router, ActivatedRoute } from '@angular/router';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {

  public product: Product;
  public id;

  constructor(private productService: ProductService,
              private router: Router,
              private route: ActivatedRoute,
              private winkelmandService: WinkelmandService) {
                this.id = this.route.snapshot.paramMap.get('id');
               }


  ngOnInit() {
    this.getProduct(this.id);
  }

  private getProduct(id) {

    id = parseInt(id, 10);

    if (isNaN(id)) {
      this.router.navigate(['/']);
      return;
    }

    this.productService.getProduct(id).subscribe(p => {
      this.product = p;
    });

  }

  addProduct() {
    this.winkelmandService.addProduct(this.product);
  }
}
