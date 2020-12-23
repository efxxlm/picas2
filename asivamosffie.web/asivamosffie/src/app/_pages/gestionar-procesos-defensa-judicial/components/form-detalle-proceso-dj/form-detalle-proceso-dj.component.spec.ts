import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDetalleProcesoDjComponent } from './form-detalle-proceso-dj.component';

describe('FormDetalleProcesoDjComponent', () => {
  let component: FormDetalleProcesoDjComponent;
  let fixture: ComponentFixture<FormDetalleProcesoDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDetalleProcesoDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDetalleProcesoDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
