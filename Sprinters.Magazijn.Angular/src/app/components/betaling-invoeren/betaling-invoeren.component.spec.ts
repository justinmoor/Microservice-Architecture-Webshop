import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormsModule, FormBuilder } from '@angular/forms';
import { BetalingInvoerenComponent } from './betaling-invoeren.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { BetalingService } from '../../services/betaling/betaling.service';

describe('BetalingInvoerenComponent', () => {
  let injector: TestBed;
  let component: BetalingInvoerenComponent;

  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ BetalingInvoerenComponent ],
      imports: [FormsModule,
        ReactiveFormsModule,
        HttpClientTestingModule],
      providers: [BetalingInvoerenComponent, BetalingService, FormBuilder]
    });

    injector = getTestBed();
    component = injector.get(BetalingInvoerenComponent);
    httpMock = injector.get(HttpTestingController);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('error should be true when request goes wrong', () => {
    
    const mockErrorResponse = { status: 500, statusText: 'Not Found' };
    const data = 'Server error';

    component.ngOnInit();

    component.betalingInvoerenForm.get("factuurnummer").setValue(123456);
    component.betalingInvoerenForm.get("bedrag").setValue(122);

    component.plaats();

    httpMock.expectOne('/api/betalingen').flush(data, mockErrorResponse);

    expect(component.error).toBeTruthy();
    expect(component.succes).toBeFalsy();
  });

  it('error should be true when request goes wrong', () => {
    
    const mockErrorResponse = { status: 200, statusText: 'Succes' };
    const data = 'Succes';

    component.ngOnInit();

    component.betalingInvoerenForm.get("factuurnummer").setValue(123456);
    component.betalingInvoerenForm.get("bedrag").setValue(122);

    component.plaats();
    
    httpMock.expectOne('/api/betalingen').flush(data, mockErrorResponse);
    expect(component.succes).toBeTruthy();
    expect(component.error).toBeFalsy();
  });
});
