import { Component, OnInit } from '@angular/core';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';

@Component({
    templateUrl: 'bestelling-succesvol.page.html',
    styleUrls: ['bestelling-succesvol.page.scss']
  })
  export class BestellingSuccesvolPage implements OnInit {
    constructor(
      private winkelMandService: WinkelmandService
    ) {

    }
    ngOnInit() {
      this.winkelMandService.resetWinkelmand();
    }
  }
