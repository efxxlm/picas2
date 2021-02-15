import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOtrosDescuentosOgRlcComponent } from './tabla-otros-descuentos-og-rlc.component';

describe('TablaOtrosDescuentosOgRlcComponent', () => {
  let component: TablaOtrosDescuentosOgRlcComponent;
  let fixture: ComponentFixture<TablaOtrosDescuentosOgRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOtrosDescuentosOgRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOtrosDescuentosOgRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
