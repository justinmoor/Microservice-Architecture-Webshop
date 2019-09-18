import { TestBed, getTestBed } from '@angular/core/testing';
import { LoginService } from './login.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Router, RouterModule } from '@angular/router';
import { JwtService } from '../jwt-service/jwt.service';
import { RouterTestingModule } from '@angular/router/testing';
import { Credentials } from 'src/app/models/credentials';
import { JwtResult } from 'src/app/models/JwtResult';

describe('LoginService', () => {
  const testToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IktsYW50IiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.cHj5LYIAv_xI8mo3wstULrB5Eluvkfc3SPDqHAiQOo0";

  let injector: TestBed;
  let service: LoginService;
  let jwtServiceMock: JwtService;
  let httpMock: HttpTestingController;

  let testCredentials: Credentials;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, RouterTestingModule
      ],
      providers: [JwtService]
    })

    injector = getTestBed();
    service = injector.get(LoginService);
    jwtServiceMock = injector.get(JwtService);
    httpMock = injector.get(HttpTestingController)

    testCredentials = {
      userName: 'TestUsername',
      password: 'Geheim123'
    }
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    const service: LoginService = TestBed.get(LoginService);
    expect(service).toBeTruthy();
  });

  describe('logOut', () => {
    it('shouldcallJwtLogoutAndRouterNaviage', () => {
      let logoutSpy = spyOn(jwtServiceMock, 'logout');
      let navigateSpy = spyOn((<any>service).router, 'navigate');

      service.logout();

      expect(logoutSpy).toHaveBeenCalled();
      expect(navigateSpy).toHaveBeenCalled();

    })
  })

  describe('login', () => {
    it('should set token in jwt service', () => {
      const dummyToken = {
        "token": testToken,
      }

      let testResult: JwtResult;
      service.logIn(testCredentials).subscribe(
        data => {
          testResult = data;
        }
      );

      const req = httpMock.expectOne('/api/authenticatie/login');

      req.flush(dummyToken);

      expect(req.request.method).toBe("POST");
      expect(testResult.token).toBe(testToken);
      expect(jwtServiceMock.token).toBe(testToken);
    });
  });


});
