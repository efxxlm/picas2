import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTramitarOrdenGiroComponent } from './form-tramitar-orden-giro.component';

describe('FormTramitarOrdenGiroComponent', () => {
  let component: FormTramitarOrdenGiroComponent;
  let fixture: ComponentFixture<FormTramitarOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTramitarOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTramitarOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
