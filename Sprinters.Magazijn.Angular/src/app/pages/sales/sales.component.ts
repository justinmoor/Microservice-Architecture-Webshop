import { Component, OnInit, isDevMode } from '@angular/core';
import { BestellingService } from 'src/app/services/bestelling/bestelling.service';
import { Bestelling } from 'src/app/models/Bestelling';
import { isNumber } from 'util';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrls: ['./sales.component.scss']
})
export class SalesComponent implements OnInit {
  bestellingen: Bestelling[] = [];
  SalesAvailable: boolean = true;
  constructor(private bestellingService: BestellingService) { }

  ngOnInit() {
    this.getData();
  }

  public getData() {
    this.bestellingService.getBestellingenBoven500Euro().subscribe(data => {
      if (isNumber(data)) {
        if (data == 0) {
          return;
        }
        this.bestellingen.forEach((bestelling, index) => {
          if (bestelling.id == data) {
            this.bestellingen.splice(index, 1);
          }
        })
      } else {
        this.bestellingen.push(data);
      }
    }, error => {
      this.SalesAvailable = false;
      if (isDevMode()) {
        console.error(error);
      }
    });
  }

}
