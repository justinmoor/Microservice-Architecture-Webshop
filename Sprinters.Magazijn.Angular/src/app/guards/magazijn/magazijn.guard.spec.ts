import { TestBed, async, inject, getTestBed } from '@angular/core/testing';

import { MagazijnGuard } from './magazijn.guard';
import { JwtService } from 'src/app/services/jwt-service/jwt.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from '@angular/router';

describe('MagazijnGuard', () => {
  let guard: MagazijnGuard;
  let injector: TestBed;
  let jwtServiceMock: JwtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [MagazijnGuard, JwtService]
    })

    injector = getTestBed();
    jwtServiceMock = injector.get(JwtService);
    guard = TestBed.get(MagazijnGuard);
  });

  it('should create magazijn guard', inject([MagazijnGuard], (guard: MagazijnGuard) => {
    expect(guard).toBeTruthy();
  }));

  it('should return true if user of magazijn logged in',() => {
    spyOn(jwtServiceMock, "isMagazijn").and.returnValue(true);

    expect(guard.canActivate()).toBeTruthy();
  });

  it('should return false if user is not magazijn',() => {
    spyOn(jwtServiceMock, "isMagazijn").and.returnValue(false);
    let navigateSpy = spyOn((<any>guard).router, 'navigate');

    expect(guard.canActivate()).toBeFalsy();
    expect(navigateSpy).toHaveBeenCalledWith(['/beheer']);
  });
 
});
