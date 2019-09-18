import { TestBed, getTestBed } from '@angular/core/testing';

import { WinkelmandCompactComponent } from './winkelmand-compact.component';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';
import { WinkelmandItem } from '../../models/winkelmand-item';

describe('WinkelmandCompactComponent', () => {
  let component: WinkelmandCompactComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        WinkelmandCompactComponent,
        EurcurrencyPipe
      ],
      providers: [
        WinkelmandCompactComponent
      ]
    });

    getTestBed();
    component = TestBed.get(WinkelmandCompactComponent);
  });

  describe('ngOnInit', () => {
    it('should calculate correct total price', () => {
      const winkelmandItem: WinkelmandItem = {
        artikelnummer: 1234,
        afbeeldingUrl: 'test.it',
        naam: 'Test product',
        prijs: 12.54,
        prijsWithBtw: 15.17,
        aantal: 2,
      };

      const winkelmandItem2: WinkelmandItem = {
        artikelnummer: 1234,
        afbeeldingUrl: 'test.it',
        naam: 'Test product',
        prijs: 14.34,
        prijsWithBtw: 17.35,
        aantal: 1,
      };
      const items: WinkelmandItem[] = [
        winkelmandItem,
        winkelmandItem2
      ];

      spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify(items));

      component.ngOnInit();

      expect(component.totaalBedrag).toEqual(47.69);
    });
  });
});
