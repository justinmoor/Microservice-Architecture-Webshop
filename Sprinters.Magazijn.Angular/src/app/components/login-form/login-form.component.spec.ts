import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';
import { LoginFormComponent } from './login-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginService } from '../../services/login-service/login.service';
import { JwtService } from '../../services/jwt-service/jwt.service';
import { and } from '@angular/router/src/utils/collection';
import { of, Observable, throwError } from 'rxjs';

describe('LoginFormComponent', () => {
  let component: LoginFormComponent;
  let fixture: ComponentFixture<LoginFormComponent>;
  let injector: TestBed;
  let loginServiceMock: LoginService;
  let jwtServiceMock: JwtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginFormComponent ],
      imports: [
        FormsModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule
      ]
    });

    injector = getTestBed();
    loginServiceMock = injector.get(LoginService);
    jwtServiceMock = injector.get(JwtService);
    fixture = TestBed.createComponent(LoginFormComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('field should be invalid when its touched and invalid', () => {

    component.ngOnInit();

    let field = "userName";
    component.loginForm.get(field).markAsTouched();
    component.loginForm.get(field).setValue("invalidvalue");

    let result: boolean = component.isFieldInvalid(field);

    expect(result).toBeTruthy();
  });

  it('field should be valid when its touched and valid', () => {

    component.ngOnInit();

    let field = "userName";
    component.loginForm.get(field).markAsTouched();
    component.loginForm.get(field).setValue("valid@value.com");

    let result: boolean = component.isFieldInvalid(field);

    expect(result).toBeFalsy();
  });

  it('login should logout when klant tries to login', () => {

    component.ngOnInit();

    component.loginForm.get('userName').setValue('test@mail.com');
    component.loginForm.get('password').setValue('password');

    spyOn(jwtServiceMock, "isKlant").and.returnValue(true);

    spyOn(loginServiceMock, "logIn").and.returnValue(of("jwtToken"));

    spyOn(loginServiceMock, 'logout')

    component.login();

    expect(loginServiceMock.logout).toHaveBeenCalledTimes(1);
  });

  it('should redirect to magazijn dashboard if logged in as magazijn', () => {

    component.ngOnInit();

    component.loginForm.get('userName').setValue('test@mail.com');
    component.loginForm.get('password').setValue('password');

    spyOn(jwtServiceMock, "isMagazijn").and.returnValue(true);

    spyOn(loginServiceMock, "logIn").and.returnValue(of("jwtToken"));

    const navigateSpy = spyOn((<any>component).router, 'navigate');

    component.login();

    expect(navigateSpy).toHaveBeenCalledWith(['/magazijn-dashboard']);
  });

  it('should redirect to sales dashboard if logged in as sales', () => {

    component.ngOnInit();

    component.loginForm.get('userName').setValue('test@mail.com');
    component.loginForm.get('password').setValue('password');

    spyOn(jwtServiceMock, "isSales").and.returnValue(true);

    spyOn(loginServiceMock, "logIn").and.returnValue(of("jwtToken"));

    const navigateSpy = spyOn((<any>component).router, 'navigate');

    component.login();

    expect(navigateSpy).toHaveBeenCalledWith(['/sales']);
  });

  it('error should be true on error', () => {

    component.ngOnInit();

    component.loginForm.get('userName').setValue('test@mail.com');
    component.loginForm.get('password').setValue('password');

    spyOn(jwtServiceMock, "isSales").and.returnValue(true);

    spyOn(loginServiceMock, "logIn").and.callFake(() => {
      return throwError(new Error("401"));
    });

    component.login();

    const result = component.error;

    expect(result).toBeTruthy();
  });

});
