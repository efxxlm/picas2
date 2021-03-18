import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionNovedadesComponent } from './accordion-novedades.component';

describe('AccordionNovedadesComponent', () => {
  let component: AccordionNovedadesComponent;
  let fixture: ComponentFixture<AccordionNovedadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccordionNovedadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccordionNovedadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
