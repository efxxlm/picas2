import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjpresupuestalRlcComponent } from './tabla-ejpresupuestal-rlc.component';

describe('TablaEjpresupuestalRlcComponent', () => {
  let component: TablaEjpresupuestalRlcComponent;
  let fixture: ComponentFixture<TablaEjpresupuestalRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjpresupuestalRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjpresupuestalRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
