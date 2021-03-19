import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOtrosDescuentosOgGbftrecComponent } from './tabla-otros-descuentos-og-gbftrec.component';

describe('TablaOtrosDescuentosOgGbftrecComponent', () => {
  let component: TablaOtrosDescuentosOgGbftrecComponent;
  let fixture: ComponentFixture<TablaOtrosDescuentosOgGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOtrosDescuentosOgGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOtrosDescuentosOgGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
