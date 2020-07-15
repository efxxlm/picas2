import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormDatosProponentesSeleccionadosComponent } from './form-datos-proponentes-seleccionados.component';

describe('FormDatosProponentesSeleccionadosComponent', () => {
  let component: FormDatosProponentesSeleccionadosComponent;
  let fixture: ComponentFixture<FormDatosProponentesSeleccionadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDatosProponentesSeleccionadosComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDatosProponentesSeleccionadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
