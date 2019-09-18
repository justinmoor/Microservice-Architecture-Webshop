import { TestBed, getTestBed } from '@angular/core/testing';

import { UserService } from './user.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { JwtService } from '../jwt-service/jwt.service';

describe('UserService', () => {
  let injector: TestBed;
  let service: UserService;
  let httpMock: HttpTestingController;
  let jwtServiceMock: JwtService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [UserService, JwtService]
    });

    injector = getTestBed();
    service = injector.get(UserService);
    jwtServiceMock = injector.get(JwtService);
    httpMock = injector.get(HttpTestingController);

  });


  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    service = TestBed.get(UserService);
    expect(service).toBeTruthy();
  });

  describe('getKlantGegevens', () => {

    it('should call api with json token user id', () => {
      const klantId = 'test-klant-id';

      spyOn(jwtServiceMock, 'getTokenUserId').and.returnValue(klantId);
      service.getKlantGegevens().subscribe();

      const req = httpMock.expectOne('/api/klanten/test-klant-id');

      expect(req.request.method).toBe('GET');
    });
  });
});
