import { TestBed, getTestBed } from '@angular/core/testing';

import { WinkelmandComponent } from './winkelmand.component';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { WinkelmandItem } from '../../models/winkelmand-item';
import { WinkelmandItemComponent } from '../winkelmand-item/winkelmand-item.component';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';

describe('WinkelmandComponent', () => {
  let injector: TestBed;
  let component: WinkelmandComponent;
  let winkelmandService: WinkelmandService;

  beforeEach(() => {

      TestBed.configureTestingModule({
          declarations: [
            WinkelmandComponent, 
            WinkelmandItemComponent, 
            EurcurrencyPipe, 
          ],
          providers: [
            WinkelmandService,
            WinkelmandComponent
          ]
      });
      injector = getTestBed();
      winkelmandService = injector.get(WinkelmandService);
      component = TestBed.get(WinkelmandComponent);

  });

  describe('NgOnInit', () => {

    it("should display correct total amount", () => {
      let items: WinkelmandItem[] = [
        new WinkelmandItem(12345, "https://test.it", "Fiets", 15.58, 18.85, 2),
        new WinkelmandItem(54321, "https://test.it", "Stuur", 12.67, 15.33,1),
      ];
      winkelmandService.items = items;

      component.ngOnInit();

      expect(component.totaalBedragZonderBtw).toBe(43.83);
      expect(component.totaalBedrag).toBe(53.03);
    });
  });

});
