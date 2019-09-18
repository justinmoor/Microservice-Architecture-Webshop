import { TestBed, getTestBed } from '@angular/core/testing';

import { RegistratieFormComponent } from './registratie-form.component';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterTestingModule } from '@angular/router/testing';
import { RegistratieService } from '../../services/registratie-service/registratie-service.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginService } from '../../services/login-service/login.service';

describe('RegistratieFormComponent', () => {

  let component: RegistratieFormComponent;
  let injector: TestBed;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        RegistratieFormComponent,
      ],
      providers: [
        FormBuilder,
        RegistratieService,
        RegistratieFormComponent,
        LoginService
      ],
      imports: [
        FormsModule,
        ReactiveFormsModule,
        RouterTestingModule,
        HttpClientTestingModule,
        NgbModule
      ]
    });

    injector = getTestBed();
    httpMock = injector.get(HttpTestingController);
    component = TestBed.get(RegistratieFormComponent);
    // loginServiceMock = TestBed.get(LoginService);

    component.ngOnInit();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('formValidation', () => {
    it('email can\'t be empty', () => {
      component.registratieForm.get('email').setValue('');

      const valid = component.registratieForm.get('email').valid;
      expect(valid).toBe(false);
    });

    it('email must have @', () => {
      component.registratieForm.get('email').setValue('henk.devriep.nl');

      const valid = component.registratieForm.get('email').valid;
      expect(valid).toBe(false);
    });

    it('email extension is not required', () => {
      component.registratieForm.get('email').setValue('henk@devriep');

      const valid = component.registratieForm.get('email').valid;
      expect(valid).toBe(true);
    });

    it('email should be valid', () => {
      component.registratieForm.get('email').setValue('henk@devriep.nl');

      const valid = component.registratieForm.get('email').valid;
      expect(valid).toBe(true);
    });

    it('password can\'t be empty', () => {
      component.registratieForm.get('wachtwoord').setValue('');

      const valid = component.registratieForm.get('wachtwoord').valid;
      expect(valid).toBe(false);
    });

    it('passwords should be the same', () => {
      component.registratieForm.get('wachtwoord').setValue('geheim123');
      component.registratieForm.get('wachtwoord_herhaal').setValue('geheim123');

      const valid = component.registratieForm.get('wachtwoord_herhaal').valid;
      expect(valid).toBe(true);
    });

    it('passwords should be the same', () => {
      component.registratieForm.get('wachtwoord').setValue('geheim123');
      component.registratieForm.get('wachtwoord_herhaal').setValue('geheim12');

      const valid = component.registratieForm.get('wachtwoord_herhaal').valid;
      expect(valid).toBe(false);
    });

    it('telefoon can\'t be empty', () => {
      component.registratieForm.get('telefoon').setValue('');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('telefoon can have no spaces', () => {
      component.registratieForm.get('telefoon').setValue('0101234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have dash', () => {
      component.registratieForm.get('telefoon').setValue('010-1234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have dash and spaces', () => {
      component.registratieForm.get('telefoon').setValue('010 - 123 45 67');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have international notation', () => {
      component.registratieForm.get('telefoon').setValue('+31101234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have international notation with dash', () => {
      component.registratieForm.get('telefoon').setValue('+3110-1234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have international notation with zero', () => {
      component.registratieForm.get('telefoon').setValue('+31(0) 10123 4567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon can have international notation with zero and spaces', () => {
      component.registratieForm.get('telefoon').setValue('+31(0)6 123 45678');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(true);
    });

    it('telefoon cannot have 2 dashes', () => {
      component.registratieForm.get('telefoon').setValue('06-1234-5678');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('telefoon cannot be longer than 10 digits', () => {
      component.registratieForm.get('telefoon').setValue('06 123456789');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('telefoon cannot be shorter than 10 digits', () => {
      component.registratieForm.get('telefoon').setValue('06 1234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('telefoon cannot have invalid operator declaration', () => {
      component.registratieForm.get('telefoon').setValue('+31(06) 123 45678');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('telefoon must have regional number', () => {
      component.registratieForm.get('telefoon').setValue('1234567');

      const valid = component.registratieForm.get('telefoon').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t be empty', () => {
      component.registratieForm.get('postcode').setValue('');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('should make postcode valid with input without spaces between numbers and letters', () => {
      component.registratieForm.get('postcode').setValue('2344AB');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('should make postcode valid with input with spaces between numbers and letters', () => {
      component.registratieForm.get('postcode').setValue('2344 AB');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('postcode is case insensitive', () => {
      component.registratieForm.get('postcode').setValue('2344 ab');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(true);
    });

    it('postcode can\'t have 2 spaces', () => {
      component.registratieForm.get('postcode').setValue('2344  AB');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t start with a 0', () => {
      component.registratieForm.get('postcode').setValue('0344 kj');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t have letters at beginning', () => {
      component.registratieForm.get('postcode').setValue('3BB2 kj');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t have numbers at end', () => {
      component.registratieForm.get('postcode').setValue('2344 56');

      const valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('postcode can\'t contain SS, SA or SD', () => {
      component.registratieForm.get('postcode').setValue('2344 SS');

      let valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);

      component.registratieForm.get('postcode').setValue('2344 sa');

      valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);

      component.registratieForm.get('postcode').setValue('2344 SD');

      valid = component.registratieForm.get('postcode').valid;
      expect(valid).toBe(false);
    });

    it('firstname (or lastname) is valid with simple name', () => {
      component.registratieForm.get('voornaam').setValue('Frits');

      const valid = component.registratieForm.get('voornaam').valid;
      expect(valid).toBe(true);
    });

    it('firstname (or lastname) can contain apostrof', () => {
      component.registratieForm.get('voornaam').setValue('Filippe D\'Gross');

      const valid = component.registratieForm.get('voornaam').valid;
      expect(valid).toBe(true);
    });

    it('firstname (or lastname) can contain a dash ', () => {
      component.registratieForm.get('voornaam').setValue('Hanz Meiner-Slutzgruppe');

      const valid = component.registratieForm.get('voornaam').valid;
      expect(valid).toBe(true);
    });

    it('firstname (or lastname) can\'t contain special characters like *, #, $', () => {
      component.registratieForm.get('voornaam').setValue('Hanz #Meiner');

      const valid = component.registratieForm.get('voornaam').valid;
      expect(valid).toBe(false);
    });

    it('firstname (or lastname) can\'t contain numbers', () => {
      component.registratieForm.get('voornaam').setValue('Hanz69');

      const valid = component.registratieForm.get('voornaam').valid;
      expect(valid).toBe(false);
    });
  });

  describe('registreer', () => {
    it('409 should set error', () => {
      const mockErrorResponse = { status: 409, statusText: 'Not Found' };
      const data = 'Not Found';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Not Found');
    });

    it('412 should set error', () => {
      const mockErrorResponse = { status: 412, statusText: 'Not Found' };
      const data = 'Not Found';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Not Found');
    });

    it('412 should set error', () => {
      const mockErrorResponse = { status: 412, statusText: 'Not Found' };
      const data = 'Not Found';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Not Found');
    });

    it('503 should set error', () => {
      const mockErrorResponse = { status: 503, statusText: 'Not Found' };
      const data = 'Not Found';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Not Found');
    });

    it('500 should set error', () => {
      const mockErrorResponse = { status: 500, statusText: 'Not Found' };
      const data = 'Not Found';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Not Found');
    });

    it('unknown code should show default message', () => {
      const mockErrorResponse = { status: 404, statusText: 'Not Found' };
      const data = 'Invalid request parameters';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data, mockErrorResponse);

      expect(component.error).toBe('Er is iets fout gegaan, probeer het later opnieuw');
    });

    it('succesfull register with succesfull login should redirect', () => {
      const data = 'Ok';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');
      const navigateSpy = spyOn((<any>component).router, 'navigate');
      const sessionSpy = spyOn(sessionStorage, 'setItem');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data);
      httpMock.expectOne('/api/authenticatie/login').flush(data);

      expect(sessionSpy).toHaveBeenCalledTimes(1);
      expect(navigateSpy).toHaveBeenCalledWith(['/']);
    });

    it('succesfull register with unsuccesfull login should redirect to inloggen', () => {
      const mockErrorResponse = { status: 404, statusText: 'Not Found' };
      const data = 'Ok';

      component.registratieForm.get('voornaam').setValue('Henk');
      component.registratieForm.get('achternaam').setValue('de Vriep');
      component.registratieForm.get('telefoon').setValue('0612345678');
      component.registratieForm.get('adresregel').setValue('Daltonlaan');
      component.registratieForm.get('plaats').setValue('Utrecht');
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('email').setValue('test@test.nl');
      component.registratieForm.get('wachtwoord').setValue('SuperGeheim');
      const navigateSpy = spyOn((<any>component).router, 'navigate');
      const sessionSpy = spyOn(sessionStorage, 'setItem');

      component.registreer();

      httpMock.expectOne('api/authenticatie/registreer').flush(data);
      httpMock.expectOne('/api/authenticatie/login').flush(data, mockErrorResponse);

      expect(sessionSpy).toHaveBeenCalledTimes(1);
      expect(navigateSpy).toHaveBeenCalledWith(['/inloggen']);
    });
  });

  describe('veldInvalid', () => {
    it('should be false when touched and invalid', () => {
      component.registratieForm.get('postcode').markAsTouched();
      component.registratieForm.get('postcode').setValue('');

      expect(component.veldInvalid('postcode')).toBeTruthy();
    });

    it('should be true when invalid', () => {
      component.registratieForm.get('postcode').setValue('1234AB');
      component.registratieForm.get('postcode').markAsTouched();

      expect(component.veldInvalid('postcode')).toBeFalsy();
    });
  });

});
