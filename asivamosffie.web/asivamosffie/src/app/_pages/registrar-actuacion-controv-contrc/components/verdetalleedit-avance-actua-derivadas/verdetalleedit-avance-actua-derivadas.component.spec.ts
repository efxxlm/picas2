import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleeditAvanceActuaDerivadasComponent } from './verdetalleedit-avance-actua-derivadas.component';

describe('VerdetalleeditAvanceActuaDerivadasComponent', () => {
  let component: VerdetalleeditAvanceActuaDerivadasComponent;
  let fixture: ComponentFixture<VerdetalleeditAvanceActuaDerivadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleeditAvanceActuaDerivadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleeditAvanceActuaDerivadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
