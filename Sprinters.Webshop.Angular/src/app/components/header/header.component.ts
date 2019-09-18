import { Component, OnInit, OnDestroy } from '@angular/core';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { Subscription } from 'rxjs';
import { JwtService } from '../../services/jwt-service/jwt.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  winkelmandItems: number;
  private subscription: Subscription;

  constructor(
    private winkelmandService: WinkelmandService,
    public jwtService: JwtService,
    private router: Router
  ) { }

  ngOnInit() {
    this.subscription = this.winkelmandService.aantal().subscribe(value => {
      console.log(value);
      this.winkelmandItems = value;
    });
  }

  logout() {
    this.jwtService.logout();
    this.router.navigate(['/']);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
