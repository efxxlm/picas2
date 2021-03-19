import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEnProcesoDeFirmasRlcComponent } from './tabla-en-proceso-de-firmas-rlc.component';

describe('TablaEnProcesoDeFirmasRlcComponent', () => {
  let component: TablaEnProcesoDeFirmasRlcComponent;
  let fixture: ComponentFixture<TablaEnProcesoDeFirmasRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEnProcesoDeFirmasRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEnProcesoDeFirmasRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
