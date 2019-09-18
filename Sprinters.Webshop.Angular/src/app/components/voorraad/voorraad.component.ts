import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-voorraad',
  templateUrl: './voorraad.component.html',
  styleUrls: ['./voorraad.component.scss']
})
export class VoorraadComponent implements OnInit {
  @Input() voorraad: number;
  voorraadKleur: string;

  constructor() { }

  ngOnInit() {
    this.voorraad = Math.min(this.voorraad, 8);
    this.getVoorraadKleur();
  }

  private getVoorraadKleur() {
    if (this.voorraad < 3) {
      this.voorraadKleur = 'red';
    } else if (this.voorraad >= 3 && this.voorraad <= 6) {
      this.voorraadKleur = 'orange';
    } else {
      this.voorraadKleur = 'green';
    }
  }
}
