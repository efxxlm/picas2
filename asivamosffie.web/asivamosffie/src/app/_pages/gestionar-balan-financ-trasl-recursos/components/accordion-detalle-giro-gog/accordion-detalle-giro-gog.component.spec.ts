import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionDetalleGiroGogComponent } from './accordion-detalle-giro-gog.component';

describe('AccordionDetalleGiroGogComponent', () => {
  let component: AccordionDetalleGiroGogComponent;
  let fixture: ComponentFixture<AccordionDetalleGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccordionDetalleGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccordionDetalleGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
