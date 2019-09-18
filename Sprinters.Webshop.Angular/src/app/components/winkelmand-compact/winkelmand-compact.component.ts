import { Component, OnInit } from '@angular/core';
import { WinkelmandItem } from '../../models/winkelmand-item';

@Component({
  selector: 'app-winkelmand-compact',
  templateUrl: './winkelmand-compact.component.html',
  styleUrls: ['./winkelmand-compact.component.scss']
})
export class WinkelmandCompactComponent implements OnInit {

  totaalBedrag = 0;
  items: WinkelmandItem[];

  constructor() { }

  ngOnInit() {
    this.items = JSON.parse(localStorage.getItem("winkelmand"));
    this.items.forEach(item => this.totaalBedrag += (item.prijsWithBtw * item.aantal))
  }
}
