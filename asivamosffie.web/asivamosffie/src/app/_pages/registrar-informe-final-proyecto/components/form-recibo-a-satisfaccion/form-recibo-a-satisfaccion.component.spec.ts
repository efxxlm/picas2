import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormReciboASatisfaccionComponent } from './form-recibo-a-satisfaccion.component';

describe('FormReciboASatisfaccionComponent', () => {
  let component: FormReciboASatisfaccionComponent;
  let fixture: ComponentFixture<FormReciboASatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormReciboASatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormReciboASatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
