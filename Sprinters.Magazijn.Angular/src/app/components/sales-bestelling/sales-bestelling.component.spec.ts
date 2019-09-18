import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';

import { SalesBestellingComponent } from './sales-bestelling.component';
import { EurcurrencyPipe } from 'src/app/pipes/eurocurrency/eurcurrency.pipe';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { BestellingService } from 'src/app/services/bestelling/bestelling.service';
import { Bestelling } from 'src/app/models/bestelling';
import { NgbModal, NgbModalRef, NgbModule } from '@ng-bootstrap/ng-bootstrap';



describe('SalesBestellingComponent', () => {
  let component: SalesBestellingComponent;

  let injector: TestBed;
  let serviceMock: BestellingService;
  let modalService: NgbModal;
  let modalRef: NgbModalRef;

  const bestelling: Bestelling = {
    id: 1,
    voornaam: 'Bob',
    achternaam: 'Kuipers',
    adres: 'Van der brugghenstraat',
    adresRegel2: '5e Verdieping',
    plaats: 'Nijmegen',
    postcode: '6511SL',
    inBehandeling: null,
    besteldeArtikelen: [{
      id: 1,
      artikel: {
        artikelnummer: 1,
        naam: 'Grote Fiets',
        beschrijving: 'Hele mooie grote fiets',
        prijs: 10,
        prijsWithBtw: 12.1,
        leverbaarVanaf: new Date(2018, 1, 1),
        leverbaarTot: new Date(2018, 1, 1),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 2
    }, {
      id: 2,
      artikel: {
        artikelnummer: 1,
        naam: 'Grote Fiets',
        beschrijving: 'Hele mooie grote fiets',
        prijs: 10,
        prijsWithBtw: 12.1,
        leverbaarVanaf: new Date(2018, 1, 1),
        leverbaarTot: new Date(2018, 1, 1),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 2
    }, {
      id: 3,
      artikel: {
        artikelnummer: 1,
        naam: 'Grote Fiets',
        beschrijving: 'Hele mooie grote fiets',
        prijs: 10,
        prijsWithBtw: 12.1,
        leverbaarVanaf: new Date(2018, 1, 1),
        leverbaarTot: new Date(2018, 1, 1),
        voorraad: 5,
      },
      artikelId: 1,
      aantal: 2
    }]
  };


  beforeEach(async(() => {


    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, NgbModule.forRoot()],
      declarations: [
        EurcurrencyPipe,
      ],
      providers: [
        BestellingService,
        SalesBestellingComponent],

    })
      .compileComponents();

    injector = getTestBed();
    serviceMock = injector.get(BestellingService);
    modalService = injector.get(NgbModal);
    modalRef = modalService.open(null);
    component = TestBed.get(SalesBestellingComponent);

  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set input value', () => {
    component.bestelling = bestelling;

    component.ngOnInit();

    expect(component.totaalVanBestelling).toBe(36.3);
  });

  describe('bestellingAfkeuren', () => {
    it('should call bestellingServiceBestellingAfkeurenWithValue1', () => {
      component.bestelling = bestelling;
      const spyServciceMockBestellingAfkeuren = spyOn(serviceMock, 'bestellingAfkeuren');

      component.bestellingAfkeuren();

      expect(spyServciceMockBestellingAfkeuren).toHaveBeenCalledWith(1);
    });
  });

  describe('bestellingGoedkeuren', () => {
    it('should call bestellingServiceBestellingAfkeurenWithValue1', () => {
      component.bestelling = bestelling;
      const spyServciceMockBestellingGoedkeuren = spyOn(serviceMock, 'bestellingGoedKeuren');

      component.bestellingGoedkeuren();

      expect(spyServciceMockBestellingGoedkeuren).toHaveBeenCalledWith(1);
    });
  });

  describe('open', () => {
    it('should call modalService open ', () => {
      const spyOnModalService = spyOn(modalService, 'open').and.returnValue(modalRef);

      component.open('test');

      expect(spyOnModalService).toHaveBeenCalled();
    });

    it('should return result goedkeuren', () => {
      component.bestelling = bestelling;

      const spyOnBestellingGoedKeuren = spyOn(component, 'bestellingGoedkeuren');

      modalRef.result = new Promise<String>((resolve, reject) => {
        resolve('goedkeuren');
        component.bestellingGoedkeuren();
      });
      spyOn(modalService, 'open').and.returnValue(modalRef);

      component.open('test');

      expect(spyOnBestellingGoedKeuren).toHaveBeenCalled();
    });

    it('should return result afkeuren', () => {
      component.bestelling = bestelling;

      const spyOnBestellingAfkeuren = spyOn(component, 'bestellingAfkeuren');

      modalRef.result = new Promise<String>((resolve, reject) => {
        resolve('afkeuren');
        component.bestellingAfkeuren();
      });
      spyOn(modalService, 'open').and.returnValue(modalRef);

      component.open('test');

      expect(spyOnBestellingAfkeuren).toHaveBeenCalled();
    });
  });

});
