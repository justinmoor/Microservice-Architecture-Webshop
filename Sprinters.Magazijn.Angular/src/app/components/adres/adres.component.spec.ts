import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';

import { AdresComponent } from './adres.component';
import { RouterTestingModule } from '@angular/router/testing';
import { Artikel } from 'src/app/models/Artikel';
import { BesteldeArtikelen } from 'src/app/models/BesteldeArtikelen';
import { Bestelling } from 'src/app/models/bestelling';
import { PrintService } from 'src/app/services/print/print.service';


describe('AdresComponent', () => {
  let component: AdresComponent;
  let fixture: ComponentFixture<AdresComponent>;
  let serviceMock: PrintService;
  let injector: TestBed;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [AdresComponent],
      imports: [RouterTestingModule]
    });

    injector = getTestBed();
    serviceMock = injector.get(PrintService);

    fixture = TestBed.createComponent(AdresComponent);
    component = fixture.componentInstance;
  }));

  it('should create', () => {
    spyOn(serviceMock, 'onDataReady').and.callFake(function () {
      console.log('Spy is called');
    });
    expect(component).toBeTruthy();
  });

  describe('ngOnint', () => {
    it('should call sessionStorage and get bestelling', () => {
      const onDataReadySpy = spyOn(serviceMock, 'onDataReady');


      const artikel: Artikel = {
        artikelnummer: 1,
        naam: 'Fiets',
        beschrijving: 'Mooie fiets',
        prijs: 52,
        prijsWithBtw: 62.92,
        leverbaarVanaf: null,
        leverbaarTot: null,
        voorraad: 5
      };
      const items: BesteldeArtikelen[] = [
        {
          id: 1,
          artikel: artikel,
          artikelId: 1,
          aantal: 5
        }, {
          id: 1,
          artikel: artikel,
          artikelId: 1,
          aantal: 5
        }];

      const bestelling: Bestelling = {
        id: 1,
        voornaam: 'Peter',
        achternaam: 'Crouch',
        adres: 'Grote straat 1',
        plaats: 'Groningen',
        postcode: '6111AA',
        inBehandeling: false,
        besteldeArtikelen: items
      };

      const bestellingJson = JSON.stringify(bestelling);

      const spySessionStorage = spyOn(sessionStorage, 'getItem').and.returnValue(bestellingJson);
      component.ngOnInit();

      expect(spySessionStorage).toHaveBeenCalledWith('bestelling');
      expect(component.bestelling).toEqual(bestelling);
    });
  });
});
