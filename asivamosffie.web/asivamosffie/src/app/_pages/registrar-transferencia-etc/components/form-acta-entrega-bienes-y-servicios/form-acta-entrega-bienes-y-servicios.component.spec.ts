import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormActaEntregaBienesYServiciosComponent } from './form-acta-entrega-bienes-y-servicios.component';

describe('FormActaEntregaBienesYServiciosComponent', () => {
  let component: FormActaEntregaBienesYServiciosComponent;
  let fixture: ComponentFixture<FormActaEntregaBienesYServiciosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormActaEntregaBienesYServiciosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormActaEntregaBienesYServiciosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
