import { BesteldGuard } from './besteld.guard';
import { TestBed, getTestBed, inject } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

describe('BesteldGuard', () => {
    let guard: BesteldGuard;

    beforeEach(() => {
      TestBed.configureTestingModule({
        imports: [RouterTestingModule],
        providers: [
            BesteldGuard,
        ]
      });

      getTestBed();
      guard = TestBed.get(BesteldGuard);
    });

    it('should create guard', inject([BesteldGuard], () => {
        expect(guard).toBeTruthy();
    }));

    describe('canActivate', () => {
        it('should return true when data in sessionstorage', () => {
            spyOn(sessionStorage, 'getItem').and.returnValue(3);

            expect(guard.canActivate()).toBeTruthy();
            expect(sessionStorage.getItem).toHaveBeenCalledWith('besteld');
        });

        it('should return false when no data in sessionstorage', () => {
            spyOn(sessionStorage, 'getItem').and.returnValue(null);
            const navigateSpy = spyOn((<any>guard).router, 'navigate');

            expect(guard.canActivate()).toBeFalsy();
            expect(sessionStorage.getItem).toHaveBeenCalledWith('besteld');
            expect(navigateSpy).toHaveBeenCalledWith(['/']);
        });
    });
});
