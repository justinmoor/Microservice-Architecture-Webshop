import { WinkelmandItem } from 'src/app/models/winkelmand-item';
import { WinkelmandItemComponent } from './winkelmand-item.component';
import { TestBed, getTestBed } from '@angular/core/testing';
import { WinkelmandService } from '../../services/winkelmand/winkelmand.service';
import { EurcurrencyPipe } from '../../pipes/eurocurrency/eurcurrency.pipe';

describe('Winkelmanditem component test', () => {
  let injector: TestBed;
  let component: WinkelmandItemComponent;
  let winkelmandServiceMock: WinkelmandService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        WinkelmandItemComponent,
        EurcurrencyPipe
      ],
      providers: [
        WinkelmandService,
        WinkelmandItemComponent
      ]
    });
    injector = getTestBed();
    winkelmandServiceMock = injector.get(WinkelmandService);
    component = TestBed.get(WinkelmandItemComponent);

  });

  describe('ngOnInit', () => {
    it('should calculate total price with and without btw', () => {
      const winkelmanditem = new WinkelmandItem(12345, 'http://test.it', 'iPhone', 6.75, 8.17, 2);

      component.item = winkelmanditem;

      component.ngOnInit();

      expect(component.totaalVanItemZonderBtw).toBe(13.50);
      expect(component.totaalVanItem).toBe(16.34);

    });
  });

  describe('addItem', () => {
    it('should call service and update totals', () => {
      const winkelmanditem = new WinkelmandItem(12345, 'http://test.it', 'iPhone', 6.75, 8.17, 2);
      component.item = winkelmanditem;
      spyOn(winkelmandServiceMock, 'addItem');

      component.addItem();

      expect(winkelmandServiceMock.addItem).toHaveBeenCalledTimes(1);
    });
  });

  describe('removeItem', () => {
    it('should call service and update totals', () => {
      const winkelmanditem = new WinkelmandItem(12345, 'http://test.it', 'iPhone', 6.75, 8.17, 2);
      component.item = winkelmanditem;
      spyOn(winkelmandServiceMock, 'removeItem');

      component.removeItem();

      expect(winkelmandServiceMock.removeItem).toHaveBeenCalledTimes(1);
    });
  });

  describe('removeProduct', () => {
    it('should call service and update totals', () => {
      const winkelmanditem = new WinkelmandItem(12345, 'http://test.it', 'iPhone', 6.75, 8.17, 2);
      component.item = winkelmanditem;
      spyOn(winkelmandServiceMock, 'removeProduct');

      component.removeProduct();

      expect(winkelmandServiceMock.removeProduct).toHaveBeenCalledTimes(1);
    });
  });
});


