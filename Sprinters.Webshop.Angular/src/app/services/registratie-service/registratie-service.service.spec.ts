import { TestBed, getTestBed, async } from '@angular/core/testing';

import { RegistratieService } from './registratie-service.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Klant } from '../../models/Klant';

describe('RegistratieServiceService', () => {
  let injector: TestBed;
  let service: RegistratieService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RegistratieService]
    });

    injector = getTestBed();
    service = injector.get(RegistratieService);
    httpMock = injector.get(HttpTestingController);

  });

  afterEach(() => {
    httpMock.verify();
  });


  it('should be created', () => {
    service = TestBed.get(RegistratieService);
    expect(service).toBeTruthy();
  });

  describe('registreer', () => {
    it('should post data', async(async () => {
      const klant: Klant = {
        email: 'test@test.nl',
        voornaam: 'henk',
        achternaam: 'de vriep',
        telefoonnummer: '0612345678',
        adresRegel: 'Jacobstraat 1',
        plaats: 'Utrecht',
        postcode: '1234AB',
        wachtwoord: 'SuperGeheim'
      };
      service.registreer(klant).subscribe();

      const req = httpMock.expectOne('api/authenticatie/registreer');

      expect(req.request.method).toBe('POST');
      expect(req.request.body).toBe(klant);
    }));
  });

});
