import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoorraadComponent } from './voorraad.component';

describe('VoorraadComponent', () => {
  let component: VoorraadComponent;
  let fixture: ComponentFixture<VoorraadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VoorraadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoorraadComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('should have max 8 voorraad', () => {
      component.voorraad = 9;
      component.ngOnInit();
      expect(component.voorraad).toBe(8);
    });

    it('should have original value when lower than 8 voorraad', () => {
      component.voorraad = 7;
      component.ngOnInit();
      expect(component.voorraad).toBe(7);
    });

    it('should have color green when vooraad 7 or more', () => {
      component.voorraad = 7;
      component.ngOnInit();
      expect(component.voorraadKleur).toBe('green');
    });

    it('should have color green when vooraad 6 or less', () => {
      component.voorraad = 6;
      component.ngOnInit();
      expect(component.voorraadKleur).toBe('orange');
    });

    it('should have color green when vooraad 2 or less', () => {
      component.voorraad = 2;
      component.ngOnInit();
      expect(component.voorraadKleur).toBe('red');
    });
  });
});
