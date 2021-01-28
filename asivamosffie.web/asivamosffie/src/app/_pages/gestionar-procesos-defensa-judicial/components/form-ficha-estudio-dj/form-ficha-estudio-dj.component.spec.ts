import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormFichaEstudioDjComponent } from './form-ficha-estudio-dj.component';

describe('FormFichaEstudioDjComponent', () => {
  let component: FormFichaEstudioDjComponent;
  let fixture: ComponentFixture<FormFichaEstudioDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormFichaEstudioDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormFichaEstudioDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
