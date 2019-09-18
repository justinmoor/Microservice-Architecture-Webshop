import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Bestelling } from '../../models/bestelling';
import { InpakService } from '../../services/inpak/inpak.service';
import { Router } from '@angular/router';
import { PrintService } from '../../services/print/print.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-magazijn-bestelling',
  templateUrl: './magazijn-bestelling.component.html',
  styleUrls: ['./magazijn-bestelling.component.scss']
})
export class MagazijnBestellingComponent implements OnInit, OnDestroy {
  
  bestelling: Bestelling;
  onPrintAdresClick: Boolean = false;
  onPrintFactuurClick: Boolean = false;
	private subscription: Subscription;

  constructor(private inpakService: InpakService, private router: Router, private printService : PrintService) { }

  ngOnInit() {
    this.getVolgendeBestelling();
  }

  ngOnDestroy() {
		this.subscription.unsubscribe();
  }
  
  getVolgendeBestelling(){
    this.subscription = this.inpakService.getVolgendeBestelling().subscribe((data) => {
      this.bestelling = data;
      this.inpakService.geenNieuweBestelling = false;
      sessionStorage["bestelling"] = JSON.stringify(this.bestelling);
    },
    err => {
      this.inpakService.geenNieuweBestelling = true;
      this.router.navigate(['/magazijn-dashboard']);
    });
  }

  next(){
    this.onPrintFactuurClick = false;
    this.onPrintAdresClick = false;
    this.inpakService.rondBestellingAf(this.bestelling.id);
    this.getVolgendeBestelling();
  }

  onPrintFactuur() {
    this.printService.printDocument('factuur');
    this.onPrintFactuurClick = true;
  }

  onPrintAdres() {
    this.printService.printDocument('adres');
    this.onPrintAdresClick = true;
  }

  activeVolgendeBestelling() {
     return this.onPrintAdresClick && this.onPrintFactuurClick
  }
}
