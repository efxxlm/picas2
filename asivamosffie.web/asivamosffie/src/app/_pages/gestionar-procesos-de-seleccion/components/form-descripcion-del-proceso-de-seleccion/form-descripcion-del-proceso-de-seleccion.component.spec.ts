import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormDescripcionDelProcesoDeSeleccionComponent } from './form-descripcion-del-proceso-de-seleccion.component';

describe('FormDescripcionDelProcesoDeSeleccionComponent', () => {
  let component: FormDescripcionDelProcesoDeSeleccionComponent;
  let fixture: ComponentFixture<FormDescripcionDelProcesoDeSeleccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDescripcionDelProcesoDeSeleccionComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDescripcionDelProcesoDeSeleccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
