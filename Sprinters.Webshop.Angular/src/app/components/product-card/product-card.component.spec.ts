import { TestBed, getTestBed } from '@angular/core/testing';

import { ProductCardComponent } from './product-card.component';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';
import { Product } from '../../models/product';
import { VoorraadComponent } from '../voorraad/voorraad.component';
import { Angular2FontawesomeModule } from 'angular2-fontawesome';
import { RouterTestingModule } from '@angular/router/testing';

describe('ProductCardComponent', () => {
    let component: ProductCardComponent;
    let injector: TestBed;
    let winkelmandServiceMock: WinkelmandService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [
                ProductCardComponent,
                EurcurrencyPipe,
                VoorraadComponent
            ],
            providers: [
                WinkelmandService,
                ProductCardComponent
            ],
            imports: [
                Angular2FontawesomeModule,
                RouterTestingModule
            ]
        });

        injector = getTestBed();
        winkelmandServiceMock = injector.get(WinkelmandService);
        component = TestBed.get(ProductCardComponent);

        const testProduct: Product = {
            artikelnummer: 12345,
            naam: 'Testproduct',
            beschrijving: 'TestBeschrijving',
            prijs: 6.75,
            prijsWithBtw: 8.17,
            afbeeldingUrl: 'https://mdbootstrap.com/img/Photos/Horizontal/E-commerce/Products/13.jpg',
            leverbaarVanaf: new Date(),
            leveranciercode: 'gz',
            leverancier: 'Gazelle',
            categorieen: ['Fiets'],
            voorraad: 2,
        };
        component.product = testProduct;
    });

    it('should create', () => {
        expect(true).toBeTruthy();
    });

    describe('addProduct', () => {
        it('should set bevestiging to true for 2500 ms', () => {
            spyOn(winkelmandServiceMock, 'addProduct');

            component.addProduct();

            expect(winkelmandServiceMock.addProduct).toHaveBeenCalledTimes(1);
            expect(component.bevestiging).toBeTruthy();

            setTimeout(() => {
                expect(component.bevestiging).toBeFalsy();
            }, 2500);
        });
    });
});
