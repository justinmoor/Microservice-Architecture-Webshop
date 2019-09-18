import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PrintService } from '../../services/print/print.service';
import { Bestelling } from '../../models/bestelling';

@Component({
  selector: 'app-adres',
  templateUrl: './adres.component.html',
  styleUrls: ['./adres.component.scss']
})
export class AdresComponent implements OnInit {
  bestelling: Bestelling;
  constructor(route: ActivatedRoute,
    private printService: PrintService) { }

  ngOnInit() {
    this.bestelling = JSON.parse(sessionStorage.getItem('bestelling'));
    this.printService.onDataReady();
  }

}
