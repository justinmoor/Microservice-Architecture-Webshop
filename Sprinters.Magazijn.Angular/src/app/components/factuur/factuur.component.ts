import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../../services/print/print.service';
import { Bestelling } from 'src/app/models/bestelling';

@Component({
  selector: 'app-factuur',
  templateUrl: './factuur.component.html',
  styleUrls: ['./factuur.component.scss']
})
export class FactuurComponent implements OnInit {
  bestelling: Bestelling;
  totaalBedragZonderBtw: number = 0;
  totaalBedrag: number = 0;

  constructor(route: ActivatedRoute,
    private printService: PrintService) { }

  ngOnInit() {
    this.printService.onDataReady();
    this.bestelling = JSON.parse(sessionStorage.getItem('bestelling'));

    this.bestelling.besteldeArtikelen.forEach(besteldeArtikel => this.totaalBedrag += besteldeArtikel.artikel.prijsWithBtw * besteldeArtikel.aantal);
    this.bestelling.besteldeArtikelen.forEach(item => this.totaalBedragZonderBtw += (item.artikel.prijs * item.aantal));

  }
}
