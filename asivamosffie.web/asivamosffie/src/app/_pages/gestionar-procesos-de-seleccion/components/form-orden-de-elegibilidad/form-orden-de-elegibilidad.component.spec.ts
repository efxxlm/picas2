import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormOrdenDeElegibilidadComponent } from './form-orden-de-elegibilidad.component';

describe('FormOrdenDeElegibilidadComponent', () => {
  let component: FormOrdenDeElegibilidadComponent;
  let fixture: ComponentFixture<FormOrdenDeElegibilidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOrdenDeElegibilidadComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOrdenDeElegibilidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
