import { TestBed, getTestBed } from '@angular/core/testing';

import { LoginService } from './login.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { JwtService } from '../jwt-service/jwt.service';
import { Credentials } from '../../models/credentials';
import { JwtResult } from 'src/app/models/JwtResult';

describe('LoginService', () => {
  const testToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJkaW9uQHRlc3QubmwiLCJqdGkiOiJjZDM5ZTE5Ny04ZWVkLTQ3NzUtYmE3MS0wODUzNWE5M2FhNDQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsIlVzZXJJZCI6ImFlOTAyMzVhLTJhYjAtNDRjZC05ZDYzLWQxMDcxYzc2OWVhMCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IktsYW50IiwiZXhwIjoxNTQ4MjQ3NjcyLCJpc3MiOiJrYW50aWxldmVyLm5sIiwiYXVkIjoia2FudGlsZXZlci5ubCJ9.cHj5LYIAv_xI8mo3wstULrB5Eluvkfc3SPDqHAiQOo0';
  let injector: TestBed;
  let service: LoginService;
  let httpMock: HttpTestingController;
  let jwtServiceMock: JwtService;

  let testCredentials: Credentials;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LoginService, JwtService]
    });

    injector = getTestBed();
    service = injector.get(LoginService);
    jwtServiceMock = injector.get(JwtService);
    httpMock = injector.get(HttpTestingController);

    testCredentials = {
      userName: 'TestUsername',
      password: 'Geheim123'
    };
  });

  it('should be created', () => {
    service = TestBed.get(LoginService);
    expect(service).toBeTruthy();
  });

  describe('login', () => {
    it('should set token in jwt service', () => {
      const dummyToken = {
        'token': testToken,
      };

      let testResult: JwtResult;
      service.logIn(testCredentials).subscribe(
        data => {
          testResult = data;
        }
      );

      const req = httpMock.expectOne('/api/authenticatie/login');

      req.flush(dummyToken);

      expect(req.request.method).toBe('POST');
      expect(testResult.token).toBe(testToken);
      expect(jwtServiceMock.token).toBe(testToken);
    });
  });

});
