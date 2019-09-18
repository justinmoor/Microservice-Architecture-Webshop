import { TestBed, getTestBed } from '@angular/core/testing';

import { JwtService } from './jwt.service';

describe('JwtService', () => {
  let service: JwtService;
  let injector: TestBed;
  let store = {};
  const klantToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IktsYW50IiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.cHj5LYIAv_xI8mo3wstULrB5Eluvkfc3SPDqHAiQOo0";
  const magazijnToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik1hZ2F6aWpuIiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.S3SfxkaJ7EGCAVRAtFAhYIq-lbUYWVbQdnfL7YmPMJg";
  const salesToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlNhbGVzIiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.kdQFFduRrn3N68gyvvlCDplM2XfK50n5lvdA_Px_OQ0";

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
    const service: JwtService = TestBed.get(JwtService);
    expect(service).toBeTruthy();
  });

  it('should set localstorage token', () => {
    service.token = "test-token";

    expect(localStorage.setItem).toHaveBeenCalledTimes(1);
    expect(store['access_token']).toBe("test-token");
  });

  it('should set localstorage token', () => {
    store['access_token'] = "test-token";

    expect(service.token).toBe('test-token');
  });

  it('should get UserId from token', () => {
    store['access_token'] = klantToken;

    let userId = service.getTokenUserId();

    expect(userId).toBe("ae90235a-2ab0-44cd-9d63-d1071c769ea0");
  });

  it('should get token expiration date', () => {
    store['access_token'] = klantToken;

    let expirationDate = service.getTokenExpirationDate();

    let expectedDate = new Date(0);
    expectedDate.setUTCSeconds(1548247672);
    expect(expirationDate).toEqual(expectedDate);
  });

  it('should remove jwt token from storage', () => {
    store['access_token'] = klantToken;

    service.logout();

    expect(store['access_token']).toEqual('null');
  });

  it('should return false with empty token', () => {
    store['access_token'] = "";

    expect(service.isLoggedIn()).toBeFalsy();
  });

  it('should return false with invalid date', () => {
    spyOn(service, 'getTokenExpirationDate').and.returnValue(undefined);

    expect(service.isLoggedIn()).toBeFalsy();
  });

  it('should call logout on expired token', () => {
    store['access_token'] = klantToken;
    var expiredDate = new Date();
    expiredDate.setDate(expiredDate.getDate() - 1);
    spyOn(service, 'getTokenExpirationDate').and.returnValue(expiredDate);
    spyOn(service, 'logout');

    expect(service.isLoggedIn()).toBeFalsy();
    expect(service.logout).toHaveBeenCalledTimes(1);
  });

  it('should call logout on expired token', () => {
    store['access_token'] = klantToken;
    var expiredDate = new Date();
    expiredDate.setDate(expiredDate.getDate() + 1);
    spyOn(service, 'getTokenExpirationDate').and.returnValue(expiredDate);
    spyOn(service, 'logout');

    expect(service.isLoggedIn()).toBeTruthy();
  });

  it('should read magazijn role', () => {
    store['access_token'] = magazijnToken;

    spyOn(service, 'isLoggedIn').and.returnValue(true);
    let isMagazijn: boolean = service.isMagazijn();

    expect(isMagazijn).toBeTruthy();
  });


  it('should read sales role', () => {
    store['access_token'] = salesToken;

    spyOn(service, 'isLoggedIn').and.returnValue(true);


    let isSales: boolean = service.isSales();

    expect(isSales).toBeTruthy();
  });

  it('should read klant role', () => {
    store['access_token'] = klantToken;

    spyOn(service, 'isLoggedIn').and.returnValue(true);

    let isKlant: boolean = service.isKlant();

    expect(isKlant).toBeTruthy();
  });

  it('should return false with incorrect role', () => {
    store['access_token'] = salesToken;

    spyOn(service, 'isLoggedIn').and.returnValue(true);

    let isMagazijn: boolean = service.isMagazijn();

    expect(isMagazijn).toBeFalsy();
  });

});
