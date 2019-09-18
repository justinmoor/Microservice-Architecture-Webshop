import { Component, OnInit } from '@angular/core';
import { InpakService } from '../../services/inpak/inpak.service';

@Component({
  selector: 'app-magazijn-dashboard',
  templateUrl: './magazijn-dashboard.page.html',
  styleUrls: ['./magazijn-dashboard.page.scss']
})
export class MagazijnDashboardPage implements OnInit {
  constructor(public inpakService: InpakService) { }

  ngOnInit() {

  }
   
}
