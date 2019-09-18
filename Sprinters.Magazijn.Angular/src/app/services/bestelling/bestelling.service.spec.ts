import { TestBed, getTestBed, async } from '@angular/core/testing';
import { BestellingService } from './bestelling.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Bestelling } from 'src/app/models/Bestelling';
import { Observable, Subscriber } from 'rxjs';

describe('BestellingService', () => {
  let injector: TestBed;
  let service: BestellingService;
  let httpMock: HttpTestingController;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BestellingService]
    })

    injector = getTestBed();
    service = injector.get(BestellingService);
    httpMock = injector.get(HttpTestingController)

  });

  afterEach(() => {
    httpMock.verify();
  });

  describe('bestellingAfkeuren', () => {
    it('should return the id one', async(async () => {
      let observer = new Observable<Bestelling[]>(subscriber => {
        service.subscriber = subscriber;
      })

      const dummyId = [2,3,4];

      let data = [];

      service.bestellingAfkeuren(1);

      observer.subscribe(id => data.push(id));

      const req = httpMock.expectOne('/api/bestellingen/sales/afkeuren/1');

      expect(req.request.method).toBe("POST");

      req.flush(dummyId);

      expect(data).toEqual([2,3,4, 1]);
    }))

    it('should return an error', async(async () => {
      let observer = new Observable<Bestelling>(subscriber => {
        service.subscriber = subscriber;
      })

      let errResponse;

      const mockErrorResponse = { status: 404, statusText: 'Bad Request' };
      const data = 'Invalid request parameters';

      service.bestellingAfkeuren(1);


      observer.subscribe(() => null, data => errResponse = data)

      httpMock.expectOne('/api/bestellingen/sales/afkeuren/1').flush(data, mockErrorResponse);

      expect(errResponse.error).toBe('Invalid request parameters');
      expect(errResponse.status).toBe(404);
    }))

  })

  describe('bestellingGoedKeuren', () => {

  })


  describe('getBestellingenBoven500Euro', () => {
    it('should return an Observable<Product[]>', async(async () => {
      const dummyBestellingen = [
        {
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
              "od": 2,
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
      ]

      let bestelling: Bestelling;

      service.getBestellingenBoven500Euro().subscribe((data) => {
        bestelling = data;
      });

      const req = httpMock.expectOne('/api/bestellingen/sales');

      expect(req.request.method).toBe("GET");

      req.flush(dummyBestellingen);

      expect(bestelling.id).toBe(1);
    }))

    it('test http error for getBestellingBoven500Euro', () => {
      let errResponse: any;

      const mockErrorResponse = { status: 404, statusText: 'Bad Request' };
      const data = 'Invalid request parameters';

      service.getBestellingenBoven500Euro().subscribe(res => null, err => errResponse = err);
      httpMock.expectOne('/api/bestellingen/sales').flush(data, mockErrorResponse);


      expect(errResponse.error).toBe('Invalid request parameters');
      expect(errResponse.status).toBe(404);
    })
  })
})
