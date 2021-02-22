import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValtotalOgGtlcComponent } from './tabla-valtotal-og-gtlc.component';

describe('TablaValtotalOgGtlcComponent', () => {
  let component: TablaValtotalOgGtlcComponent;
  let fixture: ComponentFixture<TablaValtotalOgGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValtotalOgGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValtotalOgGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
