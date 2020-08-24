import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormDatosProponentesSeleccionadosInvitacionCerradaComponent } from './form-datos-proponentes-seleccionados-invitacion-cerrada.component';

describe('FormDatosProponentesSeleccionadosInvitacionCerradaComponent', () => {
  let component: FormDatosProponentesSeleccionadosInvitacionCerradaComponent;
  let fixture: ComponentFixture<FormDatosProponentesSeleccionadosInvitacionCerradaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDatosProponentesSeleccionadosInvitacionCerradaComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDatosProponentesSeleccionadosInvitacionCerradaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
