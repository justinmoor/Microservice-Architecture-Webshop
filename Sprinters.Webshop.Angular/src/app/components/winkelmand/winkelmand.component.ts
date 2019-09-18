import { Component, OnInit } from '@angular/core';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { WinkelmandItem } from '../../models/winkelmand-item';

@Component({
  selector: 'app-winkelmand',
  templateUrl: './winkelmand.component.html',
  styleUrls: ['./winkelmand.component.scss']
})
export class WinkelmandComponent implements OnInit {
  items: WinkelmandItem[];
  totaalBedragZonderBtw = 0;
  totaalBedrag = 0;

  constructor(private winkelmandService: WinkelmandService) {
  }

  ngOnInit() {
    this.items = this.winkelmandService.items;
    this.winkelmandService.aantal().subscribe(_ => {
      this.berekentTotaal();
    });
  }

  private berekentTotaal() {
    this.totaalBedragZonderBtw = 0;
    this.totaalBedrag = 0;


    this.items.forEach(item => this.totaalBedragZonderBtw += item.prijs * item.aantal);
    this.items.forEach(item => this.totaalBedrag += item.prijsWithBtw * item.aantal);
  }

}
