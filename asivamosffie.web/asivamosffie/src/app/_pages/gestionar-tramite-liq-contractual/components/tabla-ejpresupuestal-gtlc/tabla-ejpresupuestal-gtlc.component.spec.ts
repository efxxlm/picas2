import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjpresupuestalGtlcComponent } from './tabla-ejpresupuestal-gtlc.component';

describe('TablaEjpresupuestalGtlcComponent', () => {
  let component: TablaEjpresupuestalGtlcComponent;
  let fixture: ComponentFixture<TablaEjpresupuestalGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjpresupuestalGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjpresupuestalGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
