import { BestelService } from './bestel.service';
import { TestBed, getTestBed } from '@angular/core/testing';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { Bestelling } from '../../models/bestelling';
import { WinkelmandItem } from '../../models/winkelmand-item';


describe('BestelService', () => {
    let injector: TestBed;
    let service: BestelService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule],
            providers: [BestelService]
        });

        injector = getTestBed();
        service = injector.get(BestelService);
        httpMock = injector.get(HttpTestingController);
    });

    it('should be created', () => {
        service = TestBed.get(BestelService);
        expect(service).toBeTruthy();
    });

    describe('plaatsBestelling', () => {
        it('should post with correct data', () => {
            const winkelmandItem: WinkelmandItem = {
                artikelnummer: 1234,
                afbeeldingUrl: 'test.it',
                naam: 'Test product',
                prijs: 12.00,
                prijsWithBtw: 14.52,
                aantal: 1,
            };
            const items: WinkelmandItem[] = [
                winkelmandItem
            ];

            const bestelling: Bestelling = {
                klantId: 'klantId',
                adresregel1: 'Daltonlaan 200',
                adresregel2: '4e etage',
                plaats: 'Utrecht',
                postcode: '1234AB',
                besteldeArtikelen: items
            };
            service.plaatsBestelling(bestelling).subscribe();

            const req = httpMock.expectOne('/api/bestellingen');

            expect(req.request.method).toBe('POST');
            expect(req.request.body).toBe(bestelling);
        });
    });
});
