import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormOrdenGiroSeleccionadaComponent } from './form-orden-giro-seleccionada.component';

describe('FormOrdenGiroSeleccionadaComponent', () => {
  let component: FormOrdenGiroSeleccionadaComponent;
  let fixture: ComponentFixture<FormOrdenGiroSeleccionadaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOrdenGiroSeleccionadaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOrdenGiroSeleccionadaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
