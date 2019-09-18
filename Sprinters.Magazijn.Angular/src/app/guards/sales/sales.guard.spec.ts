import { TestBed, async, inject, getTestBed } from '@angular/core/testing';

import { JwtService } from 'src/app/services/jwt-service/jwt.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';
import { SalesGuard } from './sales.guard';

describe('SalesGuard', () => {
  let guard: SalesGuard;
  let injector: TestBed;
  let jwtServiceMock: JwtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [SalesGuard, JwtService]
    })

    injector = getTestBed();
    jwtServiceMock = injector.get(JwtService);
    guard = TestBed.get(SalesGuard);
  });

  it('should create sales guard', inject([SalesGuard], (guard: SalesGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should return true if user of sales logged in',() => {
    spyOn(jwtServiceMock, "isSales").and.returnValue(true);

    expect(guard.canActivate()).toBeTruthy();
  });

  it('should return false if user is not sales',() => {
    spyOn(jwtServiceMock, "isSales").and.returnValue(false);
    let navigateSpy = spyOn((<any>guard).router, 'navigate');

    expect(guard.canActivate()).toBeFalsy();
    expect(navigateSpy).toHaveBeenCalledWith(['/beheer']);
  });

});
