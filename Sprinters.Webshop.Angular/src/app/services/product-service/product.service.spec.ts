import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';


import { ProductService } from './product.service';
import { Product } from '../../models/product';

describe('ProductService', () => {
  let injector: TestBed;
  let service: ProductService;
  let httpMock: HttpTestingController;


  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductService]
    });

    injector = getTestBed();
    service = injector.get(ProductService);
    httpMock = injector.get(HttpTestingController);

  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    service = TestBed.get(ProductService);
    expect(service).toBeTruthy();
  });

  describe('getProducts', () => {
    it('should return an Observable<Product[]>', () => {
      const dummyProducts = [
        {
          'artikelnummer': 3,
          'naam': 'Sport-100 Helmet, Red',
          'beschrijving': 'Universal fit, well-vented, lightweight , snap-on visor.',
          'prijs': 34.9900,
          'afbeeldingUrl': 'no_image_available_small.gif',
          'leverbaarVanaf': '2001-07-01T00:00:00',
          'leverbaarTot': null,
          'leveranciercode': 'HL-U509-R',
          'leverancier': 'Batavus',
          'categorieen': [
            'Accessories',
            'Helmets'
          ],
          'voorraad': 0
        },
        {
          'artikelnummer': 100,
          'naam': 'HL Fork',
          'beschrijving': 'High-performance carbon road fork with curved legs.',
          'prijs': 229.4900,
          'afbeeldingUrl': 'fork_small.gif',
          'leverbaarVanaf': '2002-07-01T00:00:00',
          'leverbaarTot': '2003-06-30T00:00:00',
          'leveranciercode': 'FK-9939',
          'leverancier': 'Eddy Merckx',
          'categorieen': [
            'Components',
            'Forks'
          ],
          'voorraad': 0
        },
        {
          'artikelnummer': 104,
          'naam': 'LL Mountain Handlebars',
          'beschrijving': 'All-purpose bar for on or off-road.',
          'prijs': 44.5400,
          'afbeeldingUrl': 'handlebar_small.gif',
          'leverbaarVanaf': '2002-07-01T00:00:00',
          'leverbaarTot': null,
          'leveranciercode': 'HB-M243',
          'leverancier': 'Union',
          'categorieen': [
            'Components',
            'Handlebars'
          ],
          'voorraad': 0
        }
      ];


      const producten: Product[] = [];


      service.getProducts().subscribe(
        data => {
          producten.push(data);
        });

      const req = httpMock.expectOne('/api/artikelen');

      expect(req.request.method).toBe('GET');

      req.flush(dummyProducts);

      expect(producten.length).toBe(3);
      expect(producten[0].naam).toBe('Sport-100 Helmet, Red');

    });
  });

  describe('getProduct', () => {
    it('should call api with json token user id', () => {
      service.getProduct(1234).subscribe();

      const req = httpMock.expectOne('api/artikelen/1234');

      expect(req.request.method).toBe('GET');
    });
  });
});
