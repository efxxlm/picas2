import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { FormEstudioDeMercadoComponent } from './form-estudio-de-mercado.component';

describe('FormEstudioDeMercadoComponent', () => {
  let component: FormEstudioDeMercadoComponent;
  let fixture: ComponentFixture<FormEstudioDeMercadoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEstudioDeMercadoComponent ],
      imports: [
        NoopAnimationsModule,
        ReactiveFormsModule,
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEstudioDeMercadoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
