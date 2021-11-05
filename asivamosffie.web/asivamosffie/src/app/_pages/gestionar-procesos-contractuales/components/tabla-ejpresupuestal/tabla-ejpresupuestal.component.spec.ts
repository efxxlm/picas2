import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjpresupuestalComponent } from './tabla-ejpresupuestal.component';

describe('TablaEjpresupuestalComponent', () => {
  let component: TablaEjpresupuestalComponent;
  let fixture: ComponentFixture<TablaEjpresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjpresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjpresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
