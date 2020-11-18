import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormReclamacionAseguradoraActuacionComponent } from './form-reclamacion-aseguradora-actuacion.component';

describe('FormReclamacionAseguradoraActuacionComponent', () => {
  let component: FormReclamacionAseguradoraActuacionComponent;
  let fixture: ComponentFixture<FormReclamacionAseguradoraActuacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormReclamacionAseguradoraActuacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormReclamacionAseguradoraActuacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
