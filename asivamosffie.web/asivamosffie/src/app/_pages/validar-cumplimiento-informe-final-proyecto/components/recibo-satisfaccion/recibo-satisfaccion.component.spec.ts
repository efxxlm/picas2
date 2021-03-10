import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReciboSatisfaccionComponent } from './recibo-satisfaccion.component';

describe('ReciboSatisfaccionComponent', () => {
  let component: ReciboSatisfaccionComponent;
  let fixture: ComponentFixture<ReciboSatisfaccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReciboSatisfaccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReciboSatisfaccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
