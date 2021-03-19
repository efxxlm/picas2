import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOtrosDescuentosOgGtlcComponent } from './tabla-otros-descuentos-og-gtlc.component';

describe('TablaOtrosDescuentosOgGtlcComponent', () => {
  let component: TablaOtrosDescuentosOgGtlcComponent;
  let fixture: ComponentFixture<TablaOtrosDescuentosOgGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOtrosDescuentosOgGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOtrosDescuentosOgGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
