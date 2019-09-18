import { TestBed, async } from '@angular/core/testing';

import { PrintService } from './print.service';
import { RouterTestingModule } from '@angular/router/testing';

describe('PrintService', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule]
    });
    spyOn(window, 'print').and.callFake(function () {
      console.log('Spy is called');
      return true;
    });
  }));
  it('should be created', () => {
    const service: PrintService = TestBed.get(PrintService);
    expect(service).toBeTruthy();
  });
});
