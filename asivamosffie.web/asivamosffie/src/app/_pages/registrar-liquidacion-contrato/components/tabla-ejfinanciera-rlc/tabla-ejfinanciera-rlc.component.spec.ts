import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjfinancieraRlcComponent } from './tabla-ejfinanciera-rlc.component';

describe('TablaEjfinancieraRlcComponent', () => {
  let component: TablaEjfinancieraRlcComponent;
  let fixture: ComponentFixture<TablaEjfinancieraRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjfinancieraRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjfinancieraRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
