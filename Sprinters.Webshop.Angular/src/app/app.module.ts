import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { MainPage } from './pages/main/main.page';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes';
import { VoorraadComponent } from './components/voorraad/voorraad.component';
import { Angular2FontawesomeModule } from 'angular2-fontawesome/angular2-fontawesome';
import { WinkelMandPage } from './pages/winkelmand/winkelmand.page';
import { WinkelmandItemComponent } from './components/winkelmand-item/winkelmand-item.component';
import { AdresInvullenPage } from './pages/adres-invullen/adres-invullen.page';
import { ToastaModule } from 'ngx-toasta';
import { BestellingSuccesvolPage } from './pages/bestelling-succesvol/bestelling-succesvol.page';
import { EurcurrencyPipe } from './pipes/eurocurrency/eurcurrency.pipe';
import { WinkelmandCompactComponent } from './components/winkelmand-compact/winkelmand-compact.component';
import { InloggenPage } from './pages/inloggen/inloggen.page';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/jwt-service/jwt.service';
import { RegistratiePage } from './pages/registratie/registratie.page';
import { RegistratieFormComponent } from './components/registratie-form/registratie-form.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AdresFormComponent } from './components/adres-form/adres-form.component';
import { WinkelmandComponent } from './components/winkelmand/winkelmand.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { ProductDetailsPage } from './pages/product-details/product-details.page';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { ZoekPage } from './pages/zoek/zoek.page';

export function getToken() {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    ProductCardComponent,
    MainPage,
    VoorraadComponent,
    WinkelMandPage,
    AdresInvullenPage,
    BestellingSuccesvolPage,
    RegistratiePage,
    WinkelmandItemComponent,
    EurcurrencyPipe,
    WinkelmandCompactComponent,
    InloggenPage,
    ProductDetailsPage,
    LoginFormComponent,
    RegistratieFormComponent,
    AdresFormComponent,
    WinkelmandComponent,
    ProductDetailComponent,
    SearchBarComponent,
    ZoekPage
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    Angular2FontawesomeModule,
    RouterModule.forRoot(routes),
    ToastaModule.forRoot(),
    JwtModule.forRoot({
      config: { tokenGetter: getToken }
    }),
    NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
