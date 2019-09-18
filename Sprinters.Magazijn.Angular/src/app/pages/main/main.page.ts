import { Component, OnInit } from '@angular/core';
import { Bestelling } from '../../models/Bestelling';

@Component({
  selector: 'app-main',
  templateUrl: './main.page.html',
  styleUrls: ['./main.page.scss']
})
export class MainPage implements OnInit {
  laatsteBestelling: Bestelling;
  constructor() { }

  ngOnInit() {

  }

}