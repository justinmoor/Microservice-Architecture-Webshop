import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';

import { FactuurComponent } from './factuur.component';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';
import { RouterTestingModule } from '@angular/router/testing';
import { Bestelling } from 'src/app/models/bestelling';
import { BesteldeArtikelen } from 'src/app/models/BesteldeArtikelen';
import { Artikel } from 'src/app/models/Artikel';
import { componentNeedsResolution } from '@angular/core/src/metadata/resource_loading';
import { PrintService } from '../../services/print/print.service';

describe('FactuurComponent', () => {
  let component: FactuurComponent;
  let fixture: ComponentFixture<FactuurComponent>;
  let serviceMock: PrintService;
  let injector: TestBed;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FactuurComponent, EurcurrencyPipe],
      imports: [RouterTestingModule],
    });

    injector = getTestBed();
    serviceMock = injector.get(PrintService);

    fixture = TestBed.createComponent(FactuurComponent);
    component = fixture.componentInstance;
  }));


  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnint', () => {
    it('should call on data ready from service', () => {

      const artikel: Artikel = {
        artikelnummer: 1,
        naam: 'Fiets',
        beschrijving: 'Mooie fiets',
        prijs: 52,
        prijsWithBtw: 62.92,
        leverbaarVanaf: new Date(2001, 1, 1),
        leverbaarTot: new Date(2001, 2, 2),
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
      component.bestelling = bestelling;

      const bestellingJson = JSON.stringify(bestelling);

      const onDataReadySpy = spyOn(serviceMock, 'onDataReady');
      const spySessionStorage = spyOn(sessionStorage, 'getItem').and.returnValue(bestellingJson);

      component.ngOnInit();

      expect(onDataReadySpy).toHaveBeenCalledTimes(1);
    });

  });
});
