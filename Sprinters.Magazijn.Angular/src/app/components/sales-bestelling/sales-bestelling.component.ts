import { Component, OnInit, Input } from '@angular/core';
import { Bestelling } from 'src/app/models/Bestelling';
import { BestellingService } from 'src/app/services/bestelling/bestelling.service';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-sales-bestelling',
  templateUrl: './sales-bestelling.component.html',
  styleUrls: ['./sales-bestelling.component.scss']
})
export class SalesBestellingComponent implements OnInit {
  @Input() bestelling: Bestelling;

  totaalVanBestelling: number = 0;

  constructor(private bestellingService: BestellingService, private modalService: NgbModal) { }

  ngOnInit() {
    this.bestelling.besteldeArtikelen.forEach(besteldeArtikel => this.totaalVanBestelling += besteldeArtikel.artikel.prijsWithBtw);
  }

  bestellingGoedkeuren() {
    this.bestellingService.bestellingGoedKeuren(this.bestelling.id);
  }

  bestellingAfkeuren() {
    this.bestellingService.bestellingAfkeuren(this.bestelling.id);
  }

  open(content) {
    this.modalService.open(
      content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
        if (result == "goedkeuren") {
          this.bestellingGoedkeuren();
        }
        if (result == 'afkeuren') {
          this.bestellingAfkeuren();
        }
      });
  }
}
