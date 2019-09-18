import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Angular2FontawesomeModule } from 'angular2-fontawesome/angular2-fontawesome';

import { MainPage } from './pages/main/main.page';
import { EurcurrencyPipe } from './pipes/eurocurrency/eurcurrency.pipe';
import { HeaderComponent } from './components/header/header.component';
import { RouterModule } from '@angular/router';
import { routes } from './app.routes';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { BestellingPage } from './pages/bestelling/bestelling.page';
import { AdresComponent } from './components/adres/adres.component';
import { FactuurComponent } from './components/factuur/factuur.component';
import { PrintLayoutComponent } from './components/print-layout/print-layout.component';
import { MagazijnDashboardPage } from './pages/magazijn-dashboard/magazijn-dashboard.page';
import { SalesComponent } from './pages/sales/sales.component';
import { SalesBestellingComponent } from './components/sales-bestelling/sales-bestelling.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MagazijnBestellingComponent } from './components/magazijn-bestelling/magazijn-bestelling.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { InloggenPage } from './pages/inloggen/inloggen.page';
import { JwtModule } from '@auth0/angular-jwt';
import { ACCESS_TOKEN_KEY } from './services/jwt-service/jwt.service';
import { BeheerPage } from './pages/beheer/beheer.page';
import { BetalingPage } from './pages/betaling/betaling.page';
import { BetalingInvoerenComponent } from './components/betaling-invoeren/betaling-invoeren.component';

export function getToken() {
  return localStorage.getItem(ACCESS_TOKEN_KEY);
}
@NgModule({
  declarations: [
    AppComponent,
    MainPage,
    EurcurrencyPipe,
    HeaderComponent,
    InloggenPage,
    BestellingPage,
    BetalingPage,
    SalesComponent,
    SalesBestellingComponent,
    MagazijnDashboardPage,
    AdresComponent,
    FactuurComponent,
    PrintLayoutComponent,
    MagazijnBestellingComponent,
    LoginFormComponent,
    BeheerPage,
    BetalingInvoerenComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    Angular2FontawesomeModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    RouterModule.forRoot(routes),
    NgbModule,
    JwtModule.forRoot({
      config: { tokenGetter: getToken }
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
