import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjpresupuestalGbftrecComponent } from './tabla-ejpresupuestal-gbftrec.component';

describe('TablaEjpresupuestalGbftrecComponent', () => {
  let component: TablaEjpresupuestalGbftrecComponent;
  let fixture: ComponentFixture<TablaEjpresupuestalGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjpresupuestalGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjpresupuestalGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
