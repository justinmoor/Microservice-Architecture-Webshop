import { TestBed, getTestBed } from '@angular/core/testing';

import { HeaderComponent } from './header.component';
import { RouterTestingModule } from '@angular/router/testing';
import { JwtService } from '../../services/jwt-service/jwt.service';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { of } from 'rxjs';
import { RouterModule } from '@angular/router';

describe('HeaderComponent', () => {

    let component: HeaderComponent;
    let injector: TestBed;
    let winkelmandService: WinkelmandService;
    let jwtService: JwtService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [
            ],
            providers: [
                HeaderComponent,
                WinkelmandService,
                JwtService,
            ],
            imports: [
                RouterTestingModule,
                RouterModule
            ]
        });
        injector = getTestBed();
        winkelmandService = injector.get(WinkelmandService);
        jwtService = injector.get(JwtService);
        component = TestBed.get(HeaderComponent);
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });

    describe('ngOnInit', () => {
        it('should set winkelmand items from service', () => {
            const aantal = 3;
            spyOn(winkelmandService, 'aantal').and.returnValue(of(aantal));

            component.ngOnInit();

            expect(component.winkelmandItems).toEqual(3);
        });
    });

    describe('logout', () => {
        it('should call jwtService logout', () => {
            spyOn(jwtService, 'logout');
            const navigateSpy = spyOn((<any>component).router, 'navigate');

            component.logout();

            expect(jwtService.logout).toHaveBeenCalledTimes(1);
            expect(navigateSpy).toHaveBeenCalledWith(['/']);
        });
    });
});
