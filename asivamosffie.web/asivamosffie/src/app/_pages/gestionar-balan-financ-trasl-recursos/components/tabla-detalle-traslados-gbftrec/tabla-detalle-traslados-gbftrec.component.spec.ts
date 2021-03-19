import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDetalleTrasladosGbftrecComponent } from './tabla-detalle-traslados-gbftrec.component';

describe('TablaDetalleTrasladosGbftrecComponent', () => {
  let component: TablaDetalleTrasladosGbftrecComponent;
  let fixture: ComponentFixture<TablaDetalleTrasladosGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDetalleTrasladosGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDetalleTrasladosGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
