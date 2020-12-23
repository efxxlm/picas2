import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDetalleProcesoPasivoDjComponent } from './form-detalle-proceso-pasivo-dj.component';

describe('FormDetalleProcesoPasivoDjComponent', () => {
  let component: FormDetalleProcesoPasivoDjComponent;
  let fixture: ComponentFixture<FormDetalleProcesoPasivoDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDetalleProcesoPasivoDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDetalleProcesoPasivoDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
