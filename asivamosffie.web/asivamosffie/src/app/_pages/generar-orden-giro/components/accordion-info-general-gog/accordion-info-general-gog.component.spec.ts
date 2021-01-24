import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccordionInfoGeneralGogComponent } from './accordion-info-general-gog.component';

describe('AccordionInfoGeneralGogComponent', () => {
  let component: AccordionInfoGeneralGogComponent;
  let fixture: ComponentFixture<AccordionInfoGeneralGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccordionInfoGeneralGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccordionInfoGeneralGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
