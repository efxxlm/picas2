import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTerceroGiroComponent } from './form-tercero-giro.component';

describe('FormTerceroGiroComponent', () => {
  let component: FormTerceroGiroComponent;
  let fixture: ComponentFixture<FormTerceroGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTerceroGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTerceroGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
