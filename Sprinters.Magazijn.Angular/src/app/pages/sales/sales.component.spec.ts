import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';

import { SalesComponent } from './sales.component';
import { SalesBestellingComponent } from 'src/app/components/sales-bestelling/sales-bestelling.component';
import { EurcurrencyPipe } from 'src/app/pipes/eurocurrency/eurcurrency.pipe';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { BestellingService } from 'src/app/services/bestelling/bestelling.service';
import { Bestelling } from 'src/app/models/bestelling';
import { of, Observable } from 'rxjs';

describe('SalesComponent', () => {
  let component: SalesComponent;
  let fixture: ComponentFixture<SalesComponent>;
  let injector: TestBed;
  let service: BestellingService;
  let httpMock: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [SalesComponent, SalesBestellingComponent, EurcurrencyPipe],
      providers: [BestellingService]
    })
      .compileComponents();
    fixture = TestBed.createComponent(SalesComponent);
    component = fixture.componentInstance;
    injector = getTestBed();
    service = injector.get(BestellingService);
    httpMock = injector.get(HttpTestingController);

    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOninit', () => {

    it('should push bestelling to list of bestellingen', () => {
      const bestelling: Bestelling = {
        id: 1,
        voornaam: 'Peter',
        achternaam: 'Crouch',
        adres: 'Grote straat 1',
        plaats: 'Groningen',
        postcode: '6111AA',
        inBehandeling: false,
        besteldeArtikelen: null
      };

      spyOn(service, 'getBestellingenBoven500Euro').and.returnValue(of(bestelling));

      component.ngOnInit();

      expect(component.bestellingen.length).toBe(1);
    });

    it('should delete item with id one of the bestellingen list', () => {
      component.bestellingen = [
        {
          id: 1,
          voornaam: 'Peter',
          achternaam: 'Crouch',
          adres: 'Grote straat 1',
          plaats: 'Groningen',
          postcode: '6111AA',
          inBehandeling: false,
          besteldeArtikelen: null
        },
        {
          id: 2,
          voornaam: 'Peter',
          achternaam: 'Crouch',
          adres: 'Grote straat 1',
          plaats: 'Groningen',
          postcode: '6111AA',
          inBehandeling: false,
          besteldeArtikelen: null
        },
        {
          id: 3,
          voornaam: 'Peter',
          achternaam: 'Crouch',
          adres: 'Grote straat 1',
          plaats: 'Groningen',
          postcode: '6111AA',
          inBehandeling: false,
          besteldeArtikelen: null
        }, ];



      spyOn(service, 'getBestellingenBoven500Euro').and.returnValue(of(1));

      component.ngOnInit();

      expect(component.bestellingen.length).toBe(2);
    });

    it('should return undefined on an id of 0', () => {
      spyOn(service, 'getBestellingenBoven500Euro').and.returnValue(of(0));

      expect(component.getData()).toBe(undefined);
    });

    it('should throw error and set salesavaialbe to false', () => {
      const mockErrorResponse = { status: 400, statusText: 'Bad Request' };
      const data = 'Invalid request parameters';

      httpMock.expectOne('/api/bestellingen/sales').flush(data, mockErrorResponse);

      component.getData();

      expect(component.SalesAvailable).toBe(false);
    });


  });

});
