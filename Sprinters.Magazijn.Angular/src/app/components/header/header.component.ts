import { Component, OnInit } from '@angular/core';
import { JwtService } from '../../services/jwt-service/jwt.service';
import { LoginService } from '../../services/login-service/login.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private jwtService: JwtService, private loginService: LoginService) { }

  ngOnInit() {
    
  }

  logOut(){
    this.loginService.logout();
  }

}
