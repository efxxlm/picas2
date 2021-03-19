import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjfinancieraGtlcComponent } from './tabla-ejfinanciera-gtlc.component';

describe('TablaEjfinancieraGtlcComponent', () => {
  let component: TablaEjfinancieraGtlcComponent;
  let fixture: ComponentFixture<TablaEjfinancieraGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjfinancieraGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjfinancieraGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
