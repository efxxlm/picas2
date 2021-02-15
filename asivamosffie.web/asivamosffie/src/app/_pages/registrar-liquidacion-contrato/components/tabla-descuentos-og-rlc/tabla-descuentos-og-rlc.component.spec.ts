import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDescuentosOgRlcComponent } from './tabla-descuentos-og-rlc.component';

describe('TablaDescuentosOgRlcComponent', () => {
  let component: TablaDescuentosOgRlcComponent;
  let fixture: ComponentFixture<TablaDescuentosOgRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDescuentosOgRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDescuentosOgRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
