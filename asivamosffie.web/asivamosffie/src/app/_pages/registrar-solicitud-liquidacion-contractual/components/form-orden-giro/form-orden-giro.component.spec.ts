import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormOrdenGiroComponent } from './form-orden-giro.component';

describe('FormOrdenGiroComponent', () => {
  let component: FormOrdenGiroComponent;
  let fixture: ComponentFixture<FormOrdenGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOrdenGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOrdenGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
