import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormDatosProponentesNuevoComponent } from './form-datos-proponentes-nuevo.component';

describe('FormDatosProponentesNuevoComponent', () => {
  let component: FormDatosProponentesNuevoComponent;
  let fixture: ComponentFixture<FormDatosProponentesNuevoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDatosProponentesNuevoComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDatosProponentesNuevoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
