import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDescuentosOgGtlcComponent } from './tabla-descuentos-og-gtlc.component';

describe('TablaDescuentosOgGtlcComponent', () => {
  let component: TablaDescuentosOgGtlcComponent;
  let fixture: ComponentFixture<TablaDescuentosOgGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDescuentosOgGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDescuentosOgGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
