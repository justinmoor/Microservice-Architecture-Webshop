import { TestBed, getTestBed } from '@angular/core/testing';

import { JwtService } from './jwt.service';

describe('JwtService', () => {
  let service: JwtService;
  let injector: TestBed;
  const store = {};

  const klantToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IktsYW50IiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.cHj5LYIAv_xI8mo3wstULrB5Eluvkfc3SPDqHAiQOo0';
  const magazijnToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hZ2F6aWpuIiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.S3SfxkaJ7EGCAVRAtFAhYIq-lbUYWVbQdnfL7YmPMJg';
  const salesToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlNhbGVzIiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.kdQFFduRrn3N68gyvvlCDplM2XfK50n5lvdA_Px_OQ0';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [

      ],
      providers: [
        JwtService
      ]
    });

    const mockLocalStorage = {
      getItem: (key: string): string => {
        return key in store ? store[key] : null;
      },
      setItem: (key: string, value: string) => {
        store[key] = `${value}`;
      },
      removeItem: (key: string) => {
        delete store[key];
      }
    };

    spyOn(localStorage, 'getItem')
      .and.callFake(mockLocalStorage.getItem);
    spyOn(localStorage, 'setItem')
      .and.callFake(mockLocalStorage.setItem);

    injector = getTestBed();
    service = injector.get(JwtService);
  });

  it('should be created', () => {
    service = TestBed.get(JwtService);
    expect(service).toBeTruthy();
  });

  describe('set token', () => {
    it('should set localstorage token', () => {
      service.token = 'test-token';

      expect(localStorage.setItem).toHaveBeenCalledTimes(1);
      expect(store['access_token']).toBe('test-token');
    });
  });

  describe('get token', () => {
    it('should set localstorage token', () => {
      store['access_token'] = 'test-token';

      expect(service.token).toBe('test-token');
    });
  });

  describe('getTokenUserId', () => {
    it('should get UserId from token', () => {
      store['access_token'] = klantToken;

      const userId = service.getTokenUserId();

      expect(userId).toBe('ae90235a-2ab0-44cd-9d63-d1071c769ea0');
    });
  });
  describe('getTokenExpirationDate', () => {
    it('should get token expiration date', () => {
      store['access_token'] = klantToken;

      const expirationDate = service.getTokenExpirationDate();

      const expectedDate = new Date(0);
      expectedDate.setUTCSeconds(1548247672);
      expect(expirationDate).toEqual(expectedDate);
    });
  });

  describe('logout', () => {
    it('should remove jwt token from storage', () => {
      store['access_token'] = klantToken;

      service.logout();

      expect(store['access_token']).toEqual('null');
    });
  });
  describe('isMagazijn', () => {
    it('should read magazijn role', () => {
      store['access_token'] = magazijnToken;

      spyOn(service, 'isLoggedIn').and.returnValue(true);

      const isMagazijn: boolean = service.isMagazijn();

      expect(isMagazijn).toBeTruthy();
    });

    it('should return false with incorrect role', () => {
      store['access_token'] = salesToken;

      const isMagazijn: boolean = service.isMagazijn();

      expect(isMagazijn).toBeFalsy();
    });

    it('should return null when not logged in', () => {
      store['access_token'] = salesToken;

      spyOn(service, 'isLoggedIn').and.returnValue(false);

      expect(service.isMagazijn()).toEqual(null);
    });
  });

  describe('isSales', () => {
    it('should read sales role', () => {
      store['access_token'] = salesToken;

      spyOn(service, 'isLoggedIn').and.returnValue(true);

      const isSales: boolean = service.isSales();

      expect(isSales).toBeTruthy();
    });

    it('should return null when not logged in', () => {
      store['access_token'] = salesToken;

      spyOn(service, 'isLoggedIn').and.returnValue(false);

      expect(service.isSales()).toEqual(null);
    });
  });

  describe('isKlant', () => {
    it('should read klant role', () => {
      store['access_token'] = klantToken;

      spyOn(service, 'isLoggedIn').and.returnValue(true);

      const isKlant: boolean = service.isKlant();

      expect(isKlant).toBeTruthy();
    });

    it('should return null when not logged in', () => {
      store['access_token'] = salesToken;

      spyOn(service, 'isLoggedIn').and.returnValue(false);

      expect(service.isKlant()).toEqual(null);
    });
  });

  describe('isLoggedIn', () => {
    it('should return false with empty token', () => {
      store['access_token'] = '';

      expect(service.isLoggedIn()).toBeFalsy();
    });

    it('should return false with invalid date', () => {
      spyOn(service, 'getTokenExpirationDate').and.returnValue(undefined);

      expect(service.isLoggedIn()).toBeFalsy();
    });

    it('should call logout on expired token', () => {
      store['access_token'] = klantToken;
      const expiredDate = new Date();
      expiredDate.setDate(expiredDate.getDate() - 1);
      spyOn(service, 'getTokenExpirationDate').and.returnValue(expiredDate);
      spyOn(service, 'logout');

      expect(service.isLoggedIn()).toBeFalsy();
      expect(service.logout).toHaveBeenCalledTimes(1);
    });

    it('should call logout on expired token', () => {
      store['access_token'] = klantToken;
      const expiredDate = new Date();
      expiredDate.setDate(expiredDate.getDate() + 1);
      spyOn(service, 'getTokenExpirationDate').and.returnValue(expiredDate);
      spyOn(service, 'logout');
    });

    it('should return true on valid token', () => {
      store['access_token'] = klantToken;
      const expiredDate = new Date();
      expiredDate.setDate(expiredDate.getDate() + 1);
      spyOn(service, 'getTokenExpirationDate').and.returnValue(expiredDate);

      expect(service.isLoggedIn()).toBeTruthy();
    });
  });
});
