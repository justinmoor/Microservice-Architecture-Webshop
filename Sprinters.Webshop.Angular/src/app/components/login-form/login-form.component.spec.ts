import { TestBed, getTestBed } from '@angular/core/testing';

import { LoginFormComponent } from './login-form.component';
import { FormsModule, ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginService } from '../../services/login-service/login.service';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let injector: TestBed;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        LoginFormComponent
      ],
      providers: [
        LoginFormComponent,
        LoginService,
        FormBuilder
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
    component = TestBed.get(LoginFormComponent);
    httpMock = injector.get(HttpTestingController);
    component.ngOnInit();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should get message if exist', () => {
      spyOn(sessionStorage, 'getItem').and.returnValue('message');

      component.ngOnInit();

      expect(component.message).toEqual('message');
    });
  });

  describe('isFieldInvalid', () => {
    it('should be false when touched and invalid', () => {
      component.loginForm.get('userName').markAsTouched();
      component.loginForm.get('userName').setValue('');

      expect(component.isFieldInvalid('userName')).toBeTruthy();
    });

    it('should be true when invalid', () => {
      component.loginForm.get('userName').setValue('test@test.nl');
      component.loginForm.get('userName').markAsTouched();

      expect(component.isFieldInvalid('userName')).toBeFalsy();
    });
  });

  describe('login', () => {
    it('should call login with correct credentials', () => {
      component.loginForm.get('userName').setValue('test@test.nl');
      component.loginForm.get('password').setValue('Geheim123!');

      component.login();

      const req = httpMock.expectOne('/api/authenticatie/login');

      expect(req.request.body['userName']).toEqual('test@test.nl');
      expect(req.request.body['password']).toEqual('Geheim123!');
    });

    it('should redirect user with correct data', () => {
      component.loginForm.get('userName').setValue('test@test.nl');
      component.loginForm.get('password').setValue('Geheim123!');
      const sessionSpy = spyOn(sessionStorage, 'setItem');
      const navigateSpy = spyOn((<any>component).router, 'navigate');

      component.login();

      httpMock.expectOne('/api/authenticatie/login').flush('ok');

      expect(sessionSpy).toHaveBeenCalledTimes(1);
      expect(navigateSpy).toHaveBeenCalledWith(['/']);
    });

    it('should set error on invalid response', () => {
      component.loginForm.get('userName').setValue('test@test.nl');
      component.loginForm.get('password').setValue('Geheim123!');

      component.login();

      httpMock.expectOne('/api/authenticatie/login').flush('ok', {status: 404, statusText: 'Error'});

      expect(component.error).toBeTruthy();
    });
  });

});
