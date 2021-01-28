import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormGenerarOrdenGiroComponent } from './form-generar-orden-giro.component';

describe('FormGenerarOrdenGiroComponent', () => {
  let component: FormGenerarOrdenGiroComponent;
  let fixture: ComponentFixture<FormGenerarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormGenerarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormGenerarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
