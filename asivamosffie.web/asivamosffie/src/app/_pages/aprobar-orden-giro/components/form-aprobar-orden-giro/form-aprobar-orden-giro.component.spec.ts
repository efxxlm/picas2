import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAprobarOrdenGiroComponent } from './form-aprobar-orden-giro.component';

describe('FormAprobarOrdenGiroComponent', () => {
  let component: FormAprobarOrdenGiroComponent;
  let fixture: ComponentFixture<FormAprobarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAprobarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAprobarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
