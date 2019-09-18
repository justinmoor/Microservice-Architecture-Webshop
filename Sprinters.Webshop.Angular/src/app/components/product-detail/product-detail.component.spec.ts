import { TestBed, getTestBed } from '@angular/core/testing';

import { ProductDetailComponent } from './product-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { ProductService } from '../../services/product-service/product.service';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { VoorraadComponent } from '../voorraad/voorraad.component';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';
import { Angular2FontawesomeModule } from 'angular2-fontawesome';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Product } from '../../models/product';
import { of } from 'rxjs';

describe('ProductDetailComponent', () => {
    let component: ProductDetailComponent;
    let injector: TestBed;
    let winkelmandServiceMock: WinkelmandService;
    let productServiceMock: ProductService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [
                ProductDetailComponent,
                VoorraadComponent,
                EurcurrencyPipe
            ],
            providers: [
                ProductDetailComponent,
                ProductService,
                WinkelmandService
            ],
            imports: [
                Angular2FontawesomeModule,
                HttpClientTestingModule,
                RouterTestingModule
            ]
        });

        injector = getTestBed();
        component = TestBed.get(ProductDetailComponent);
        winkelmandServiceMock = injector.get(WinkelmandService);
        productServiceMock = injector.get(ProductService);

    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('ngOnInit', () => {

        it('should redirect if no product present', () => {
            const navigateSpy = spyOn((<any>component).router, 'navigate');

            component.ngOnInit();

            expect(navigateSpy).toHaveBeenCalledWith(['/']);
        });

        it('should redirect if id not a number', () => {

            component.id = 'nan';

            const navigateSpy = spyOn((<any>component).router, 'navigate');

            component.ngOnInit();

            expect(navigateSpy).toHaveBeenCalledWith(['/']);
        });

        it('should should call service on valid id', () => {
            const item: Product = {
                artikelnummer: 1234,
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

            component.id = 1234;
            spyOn(productServiceMock, 'getProduct').and.returnValue(of(item));

            component.ngOnInit();

            expect(component.product).toEqual(item);
            expect(productServiceMock.getProduct).toHaveBeenCalledTimes(1);
        });
    });

    describe('addProduct', () => {

        it('should call service with data', () => {
            const item: Product = {
                artikelnummer: 1234,
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

            component.product = item;
            spyOn(winkelmandServiceMock, 'addProduct');

            component.addProduct();

            expect(winkelmandServiceMock.addProduct).toHaveBeenCalledWith(item);
        });

    });
});
