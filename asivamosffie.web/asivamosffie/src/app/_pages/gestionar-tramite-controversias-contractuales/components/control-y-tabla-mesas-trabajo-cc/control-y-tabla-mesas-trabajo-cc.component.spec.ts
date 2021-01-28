import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaMesasTrabajoCcComponent } from './control-y-tabla-mesas-trabajo-cc.component';

describe('ControlYTablaMesasTrabajoCcComponent', () => {
  let component: ControlYTablaMesasTrabajoCcComponent;
  let fixture: ComponentFixture<ControlYTablaMesasTrabajoCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaMesasTrabajoCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaMesasTrabajoCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
