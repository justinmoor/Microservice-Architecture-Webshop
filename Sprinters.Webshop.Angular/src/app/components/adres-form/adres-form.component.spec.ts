import { TestBed, getTestBed } from '@angular/core/testing';

import { AdresFormComponent } from './adres-form.component';
import { UserService } from '../../services/user-service/user.service';
import { WinkelmandCompactComponent } from '../winkelmand-compact/winkelmand-compact.component';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BestelService } from '../../services/bestel-service/bestel.service';
import { HttpClient } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';
import { Klant } from '../../models/Klant';
import { of } from 'rxjs';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { WinkelmandItem } from '../../models/winkelmand-item';

describe('AdresFormComponent', () => {
  let component: AdresFormComponent;
  let injector: TestBed;
  let serviceMock: UserService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        AdresFormComponent,
        WinkelmandCompactComponent,
        EurcurrencyPipe,
      ],
      providers: [
        FormBuilder,
        BestelService,
        HttpClient,
        AdresFormComponent,
      ],
      imports: [
        FormsModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule
      ]
    });

    const klant: Klant = {
      voornaam: 'Kees',
      achternaam: 'de Koning',
      telefoonnummer: '0612345678',
      adresRegel: 'Daltonlaan 200',
      plaats: 'Utrecht',
      postcode: '1234AB',
      email: 'keesdekoning@gmail.com'
    };
    injector = getTestBed();
    serviceMock = injector.get(UserService);
    component = TestBed.get(AdresFormComponent);
    httpMock = injector.get(HttpTestingController);
    spyOn(serviceMock, 'getKlantGegevens').and.returnValue(of(klant));

    component.ngOnInit();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('formValidation', () => {
    it('postcode can\'t be empty', () => {
      component.adresForm.get('postcode').setValue('');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('should make postcode valid with input without spaces between numbers and letters', () => {
      component.adresForm.get('postcode').setValue('2344AB');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('should make postcode valid with input with spaces between numbers and letters', () => {
      component.adresForm.get('postcode').setValue('2344 AB');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('postcode is case insensitive', () => {
      component.adresForm.get('postcode').setValue('2344 ab');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('postcode can\'t have 2 spaces', () => {
      component.adresForm.get('postcode').setValue('2344  AB');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t start with a 0', () => {
      component.adresForm.get('postcode').setValue('0344 kj');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t have letters at beginning', () => {
      component.adresForm.get('postcode').setValue('3BB2 kj');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t have numbers at end', () => {
      component.adresForm.get('postcode').setValue('2344 56');

      const valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t contain SS, SA or SD', () => {
      component.adresForm.get('postcode').setValue('2344 SS');

      let valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);

      component.adresForm.get('postcode').setValue('2344 sa');

      valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);

      component.adresForm.get('postcode').setValue('2344 SD');

      valid = component.adresForm.get('postcode').valid;
      expect(valid).toBe(false);
    });
  });

  describe('veldInvalid', () => {
    it('should be false when touched and invalid', () => {
      component.adresForm.get('postcode').markAsTouched();
      component.adresForm.get('postcode').setValue('');

      expect(component.veldInvalid('postcode')).toBeTruthy();
    });

    it('should be true when invalid', () => {
      component.adresForm.get('postcode').setValue('1234AB');
      component.adresForm.get('postcode').markAsTouched();

      expect(component.veldInvalid('postcode')).toBeFalsy();
    });
  });

  describe('plaatsBestelling', () => {
    it('should strip spaces from postcode', () => {
      component.adresForm.get('postcode').setValue('1234 AB');
      component.plaatsBestelling();

      const req = httpMock.expectOne('/api/bestellingen');

      expect(req.request.body['postcode']).toEqual('1234AB');
    });


    it('should set error to false on http error', () => {
      const mockErrorResponse = { status: 412, statusText: 'Not Found' };
      const data = 'Not Found';

      component.plaatsBestelling();

      const req = httpMock.expectOne('/api/bestellingen');
      req.flush(data, mockErrorResponse);

      expect(component.error).toBeTruthy();
    });

    it('should set error to false on http error', () => {
      const sucessResponse = { status: 200, statusText: 'ok' };
      const data = 'ok';
      const navigateSpy = spyOn((<any>component).router, 'navigate');
      const sessionSpy = spyOn(sessionStorage, 'setItem');

      component.plaatsBestelling();

      const req = httpMock.expectOne('/api/bestellingen');
      req.flush(data, sucessResponse);

      expect(component.error).toBeFalsy();
      expect(navigateSpy).toHaveBeenCalledWith(['/bestellingsuccesvol']);
      expect(sessionSpy).toHaveBeenCalledTimes(1);
    });
  });

  describe('zijnWinkelmandItemsAanwezig', () => {
    it('should return false when no items present', () => {
      expect(component.zijnWinkelmandItemsAanwezig()).toBeFalsy();
    });

    it('should return true when items present', () => {
      const winkelmandItem: WinkelmandItem = {
        artikelnummer: 1234,
        afbeeldingUrl: 'test.it',
        naam: 'Test product',
        prijs: 12.00,
        prijsWithBtw: 14.52,
        aantal: 1,
      };
      const items: WinkelmandItem[] = [
        winkelmandItem
      ];

      component.items = items;

      expect(component.zijnWinkelmandItemsAanwezig()).toBeTruthy();
    });
  });

});
