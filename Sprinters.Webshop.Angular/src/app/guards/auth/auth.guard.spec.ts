import { TestBed, inject, getTestBed } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { RouterTestingModule } from '@angular/router/testing';
import { JwtService } from '../../services/jwt-service/jwt.service';

describe('AuthGuard', () => {
  let guard: AuthGuard;

  let injector: TestBed;
  let jwtServiceMock: JwtService;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [
        AuthGuard,
        JwtService,
      ]
    });

    injector = getTestBed();
    jwtServiceMock = injector.get(JwtService);
    guard = TestBed.get(AuthGuard);
  });

  it('should create guard', inject([AuthGuard], () => {
    expect(guard).toBeTruthy();
  }));

  describe('canActivate', () => {
    it('should return true if user logged in', () => {
      spyOn(jwtServiceMock, 'isLoggedIn').and.returnValue(true);

      expect(guard.canActivate()).toEqual(true);
    });

    it('should return false if user logged in', () => {
      spyOn(jwtServiceMock, 'isLoggedIn').and.returnValue(false);
      const navigateSpy = spyOn((<any>guard).router, 'navigate');

      expect(guard.canActivate()).toEqual(false);
      expect(navigateSpy).toHaveBeenCalledWith(['/inloggen']);
    });
  });

});
