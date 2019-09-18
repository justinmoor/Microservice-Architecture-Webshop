import { TestBed, getTestBed, async } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'

import { InpakService } from './inpak.service';
import { Bestelling } from '../../models/bestelling';
import { RouterTestingModule } from '@angular/router/testing';

describe('InpakService', () => {
  let injector: TestBed;
  let service: InpakService;
  let httpMock: HttpTestingController;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterTestingModule],
      providers: [InpakService]
    });

    injector = getTestBed();
    service = injector.get(InpakService);
    httpMock = injector.get(HttpTestingController)

  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('Http get request should return correct object', () => {

    it('should return an Observable<Product[]>', async(async () => {
      const dummyBestelling = {
        "id": 1,
        "voornaam": "Hans",
        "achternaam": "Van Huizen",
        "adres": "Laagstraat 11",
        "adresRegel2": null,
        "plaats": "Laaghoven",
        "postcode": "1234FG",
        "inBehandeling": false,
        "besteldeArtikelen": [
          {
            "id": 1,
            "artikel": {
              "artikelnummer": 1,
              "naam": "Fiets",
              "beschrijving": "Grote fiets voor iedereen",
              "prijs": 299.3,
              "leverbaarVanaf": "2017-01-01T00:00:00",
              "leverbaarTot": "2018-05-05T00:00:00",
              "voorraad": 5
            },
            "artikelId": 1,
            "aantal": 3
          },
          {
            "id": 2,
            "artikel": {
              "artikelnummer": 2,
              "naam": "Fiets Groot",
              "beschrijving": "HELE grote fiets voor iedereen",
              "prijs": 299.3,
              "leverbaarVanaf": "2017-01-01T00:00:00",
              "leverbaarTot": "2018-05-05T00:00:00",
              "voorraad": 8
            },
            "artikelId": 2,
            "aantal": 5
          }
        ]
      }

      let bestelling: Bestelling;

      service.getVolgendeBestelling().subscribe(
        data => {
          bestelling = data;
        }
      );

      const req = httpMock.expectOne('/api/bestellingen/next');

      req.flush(dummyBestelling);

      expect(req.request.method).toBe("GET");

      expect(bestelling.id).toBe(1);
    }));

    it('should send id to endpoint', async(async () => {

      service.rondBestellingAf(1);

      const req = httpMock.expectOne('/api/bestellingen/finish/1');

      expect(req.request.method).toBe("POST");
      expect(req.request.body).toBe(1);
    }));

  });
});