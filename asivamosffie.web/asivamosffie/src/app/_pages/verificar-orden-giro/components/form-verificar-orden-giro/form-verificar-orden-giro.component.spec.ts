import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormVerificarOrdenGiroComponent } from './form-verificar-orden-giro.component';

describe('FormVerificarOrdenGiroComponent', () => {
  let component: FormVerificarOrdenGiroComponent;
  let fixture: ComponentFixture<FormVerificarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormVerificarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormVerificarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
