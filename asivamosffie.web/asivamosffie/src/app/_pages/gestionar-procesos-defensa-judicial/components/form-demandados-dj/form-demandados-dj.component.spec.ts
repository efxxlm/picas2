import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDemandadosDjComponent } from './form-demandados-dj.component';

describe('FormDemandadosDjComponent', () => {
  let component: FormDemandadosDjComponent;
  let fixture: ComponentFixture<FormDemandadosDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDemandadosDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDemandadosDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
