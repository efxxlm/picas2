import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDescuentosOgGbftrecComponent } from './tabla-descuentos-og-gbftrec.component';

describe('TablaDescuentosOgGbftrecComponent', () => {
  let component: TablaDescuentosOgGbftrecComponent;
  let fixture: ComponentFixture<TablaDescuentosOgGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDescuentosOgGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDescuentosOgGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
