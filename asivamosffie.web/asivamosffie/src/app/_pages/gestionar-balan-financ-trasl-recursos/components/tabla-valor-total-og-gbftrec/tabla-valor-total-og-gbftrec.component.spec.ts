import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValorTotalOgGbftrecComponent } from './tabla-valor-total-og-gbftrec.component';

describe('TablaValorTotalOgGbftrecComponent', () => {
  let component: TablaValorTotalOgGbftrecComponent;
  let fixture: ComponentFixture<TablaValorTotalOgGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValorTotalOgGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValorTotalOgGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
