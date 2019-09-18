import { Component, OnInit, Input } from '@angular/core';
import { WinkelmandItem } from 'src/app/models/winkelmand-item';
import { WinkelmandService } from 'src/app/services/winkelmand/winkelmand.service';

@Component({
  selector: 'app-winkelmand-item',
  templateUrl: './winkelmand-item.component.html',
  styleUrls: ['./winkelmand-item.component.scss']
})
export class WinkelmandItemComponent implements OnInit {

  @Input() item: WinkelmandItem;

  totaalVanItem = 0;
  totaalVanItemZonderBtw = 0;

  constructor(private winkelmandService: WinkelmandService) { }

  ngOnInit() {
    this.updateAantal();
  }

  addItem() {
    this.winkelmandService.addItem(this.item);
    this.updateAantal();
  }

  removeItem() {
    this.winkelmandService.removeItem(this.item);
    this.updateAantal();
  }

  removeProduct() {
    this.winkelmandService.removeProduct(this.item);
    this.updateAantal();
  }

  private updateAantal() {
    this.totaalVanItemZonderBtw = this.item.aantal * this.item.prijs;
    this.totaalVanItem = this.item.aantal * this.item.prijsWithBtw;
  }
}
