import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormSeleccionProponenteAInvitarComponent } from './form-seleccion-proponente-a-invitar.component';

describe('FormSeleccionProponenteAInvitarComponent', () => {
  let component: FormSeleccionProponenteAInvitarComponent;
  let fixture: ComponentFixture<FormSeleccionProponenteAInvitarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormSeleccionProponenteAInvitarComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormSeleccionProponenteAInvitarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
