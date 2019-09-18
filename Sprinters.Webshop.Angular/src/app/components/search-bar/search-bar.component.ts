import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product-service/product.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.scss']
})
export class SearchBarComponent implements OnInit {
  query: string = '';

  constructor(private ps: ProductService, private router: Router) { }

  ngOnInit() {
  }

  zoek() {
    if (this.query == '') {
      this.router.navigate(['/'])
      return;
    }
    if (this.ps.filterProducts(this.query).length === 0) {
      this.router.navigate(['/'])
      return;
    }
    this.router.navigate(['/zoeken']);


  }

}
