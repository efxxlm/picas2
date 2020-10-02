import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOtrosDescuentosComponent } from './tabla-otros-descuentos.component';

describe('TablaOtrosDescuentosComponent', () => {
  let component: TablaOtrosDescuentosComponent;
  let fixture: ComponentFixture<TablaOtrosDescuentosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOtrosDescuentosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOtrosDescuentosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
