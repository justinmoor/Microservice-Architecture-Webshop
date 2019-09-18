import { TestBed, async, inject, getTestBed } from '@angular/core/testing';

import { JwtService } from 'src/app/services/jwt-service/jwt.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { KlantGuard } from './klant.guard';

describe('KlantGuard', () => {
  let guard: KlantGuard;
  let injector: TestBed;
  let jwtServiceMock: JwtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [KlantGuard, JwtService]
    })

    injector = getTestBed();
    jwtServiceMock = injector.get(JwtService);
    guard = TestBed.get(KlantGuard);
  });

  it('should create sales guard', inject([KlantGuard], (guard: KlantGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should return false if user is klant',() => {
    spyOn(jwtServiceMock, "isKlant").and.returnValue(false);
    let navigateSpy = spyOn((<any>guard).router, 'navigate');

    expect(guard.canActivate()).toBeFalsy();
    expect(navigateSpy).toHaveBeenCalledWith(['/inloggen']);
  });

  it('should return false if user is klant',() => {
    spyOn(jwtServiceMock, "isKlant").and.returnValue(true);
    spyOn(jwtServiceMock, "isLoggedIn").and.returnValue(true);

    let navigateSpy = spyOn((<any>guard).router, 'navigate');

    expect(guard.canActivate()).toBeFalsy();
    expect(navigateSpy).toHaveBeenCalledWith(['/inloggen']);
  });

  it('should return false if user not logged in',() => {
    spyOn(jwtServiceMock, "isKlant").and.returnValue(false);
    spyOn(jwtServiceMock, "isLoggedIn").and.returnValue(false);

    let navigateSpy = spyOn((<any>guard).router, 'navigate');

    expect(guard.canActivate()).toBeFalsy();
    expect(navigateSpy).toHaveBeenCalledWith(['/inloggen']);
  });

});
