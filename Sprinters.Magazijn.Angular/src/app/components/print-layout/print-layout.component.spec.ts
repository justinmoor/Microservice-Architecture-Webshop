import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrintLayoutComponent } from './print-layout.component';
import { RouterTestingModule } from '@angular/router/testing';

describe('PrintLayoutComponent', () => {
  let component: PrintLayoutComponent;
  let fixture: ComponentFixture<PrintLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintLayoutComponent ],
      imports: [RouterTestingModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
