import { getTestBed, ComponentFixture, TestBed } from '@angular/core/testing';

import { MagazijnBestellingComponent } from './magazijn-bestelling.component';
import { RouterTestingModule } from '@angular/router/testing';
import { InpakService } from '../../services/inpak/inpak.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Bestelling } from '../../models/bestelling';
import { of, throwError } from 'rxjs';
import { BesteldeArtikelen } from '../../models/BesteldeArtikelen';

describe('MagazijnBestellingComponent', () => {
  let component: MagazijnBestellingComponent;

  let injector: TestBed;
  let serviceMock: InpakService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule],
      providers: [
        MagazijnBestellingComponent,
        InpakService,
      ]
    });

    injector = getTestBed();
    serviceMock = injector.get(InpakService);
    component = TestBed.get(MagazijnBestellingComponent);

  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should get data onInit', () => {
    const artikelen: BesteldeArtikelen = {
      id: 1,
      artikel: {
        artikelnummer: 1,
        naam: 'Fiets',
        beschrijving: 'Grote fiets voor iedereen',
        prijs: 299.3,
        prijsWithBtw: 362.15,
        leverbaarVanaf: new Date(),
        leverbaarTot: new Date(),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 3
    };
    const bestelling: Bestelling = {
      id: 1,
      voornaam: 'Hans',
      achternaam: 'Van Huizen',
      adres: 'Laagstraat 11',
      adresRegel2: null,
      plaats: 'Laaghoven',
      postcode: '1234FG',
      inBehandeling: false,
      besteldeArtikelen: [
        artikelen
      ]
    };
    spyOn(serviceMock, 'getVolgendeBestelling').and.returnValue(of(bestelling));
    component.getVolgendeBestelling();
    expect(component.bestelling.id).toBe(1);

  });

  it('should redirect to dashboard on 404', () => {

    spyOn(serviceMock, 'getVolgendeBestelling').and.callFake(() => {
      return throwError(new Error('Fake error'));
    });
    const navigateSpy = spyOn((<any>component).router, 'navigate');
    component.getVolgendeBestelling();

    expect(navigateSpy).toHaveBeenCalledWith(['/magazijn-dashboard']);

  });

  it('should call rondBestellingAf with correct data', () => {
    const artikelen: BesteldeArtikelen = {
      id: 1,
      artikel: {
        artikelnummer: 1,
        naam: 'Fiets',
        beschrijving: 'Grote fiets voor iedereen',
        prijs: 299.3,
        prijsWithBtw: 362.15,
        leverbaarVanaf: new Date(),
        leverbaarTot: new Date(),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 3
    };
    component.bestelling = {
      id: 1,
      voornaam: 'Hans',
      achternaam: 'Van Huizen',
      adres: 'Laagstraat 11',
      adresRegel2: null,
      plaats: 'Laaghoven',
      postcode: '1234FG',
      inBehandeling: false,
      besteldeArtikelen: [
        artikelen
      ]
    };
    const postSpy = spyOn(serviceMock, 'rondBestellingAf');
    component.next();

    expect(postSpy).toHaveBeenCalledWith(1);
  });

  it('should call rondBestellingAf and get next data', () => {
    const artikel: BesteldeArtikelen = {
      id: 1,
      artikel: {
        artikelnummer: 1,
        naam: 'Fiets',
        beschrijving: 'Grote fiets voor iedereen',
        prijs: 299.3,
        prijsWithBtw: 362.15,
        leverbaarVanaf: new Date(),
        leverbaarTot: new Date(),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 3
    };
    component.bestelling = {
      id: 1,
      voornaam: 'Hans',
      achternaam: 'Van Huizen',
      adres: 'Laagstraat 11',
      adresRegel2: null,
      plaats: 'Laaghoven',
      postcode: '1234FG',
      inBehandeling: false,
      besteldeArtikelen: [
        artikel
      ]
    };

    const artikelen2: BesteldeArtikelen = {
      id: 2,
      artikel: {
        artikelnummer: 2,
        naam: 'Fiets',
        beschrijving: 'Grote fiets voor iedereen',
        prijs: 299.3,
        prijsWithBtw: 362.15,
        leverbaarVanaf: new Date(),
        leverbaarTot: new Date(),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 3
    };
    const bestelling2: Bestelling = {
      id: 2,
      voornaam: 'Hans',
      achternaam: 'Van Huizen',
      adres: 'Laagstraat 11',
      adresRegel2: null,
      plaats: 'Laaghoven',
      postcode: '1234FG',
      inBehandeling: false,
      besteldeArtikelen: [
        artikelen2
      ]
    };
    spyOn(serviceMock, 'rondBestellingAf');
    spyOn(serviceMock, 'getVolgendeBestelling').and.returnValue(of(bestelling2));
    component.next();

    expect(component.bestelling.id).toBe(2);
  });

  it('should active Volgende bestelling button', () => {
    component.onPrintAdres();
    component.onPrintFactuur();
    expect(component.onPrintAdresClick).toBe(true);
    expect(component.onPrintFactuurClick).toBe(true);
  });

});
