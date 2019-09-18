import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchBarComponent } from './search-bar.component';

import { FormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ProductService } from 'src/app/services/product-service/product.service';
import { RouterTestingModule } from '@angular/router/testing';

describe('SearchBarComponent', () => {
  let component: SearchBarComponent;
  let fixture: ComponentFixture<SearchBarComponent>;


  let injector: TestBed;


  beforeEach(async(() => {
    TestBed.configureTestingModule({
      providers: [ProductService],
      declarations: [SearchBarComponent],
      imports: [
        FormsModule,
        HttpClientTestingModule,
        RouterTestingModule
      ]
    })
      .compileComponents();



  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe
});
