import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormTerceroGiroGogComponent } from './form-tercero-giro-gog.component';

describe('FormTerceroGiroGogComponent', () => {
  let component: FormTerceroGiroGogComponent;
  let fixture: ComponentFixture<FormTerceroGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormTerceroGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormTerceroGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
