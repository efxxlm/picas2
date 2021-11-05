import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjfinancieraComponent } from './tabla-ejfinanciera.component';

describe('TablaEjfinancieraComponent', () => {
  let component: TablaEjfinancieraComponent;
  let fixture: ComponentFixture<TablaEjfinancieraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjfinancieraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjfinancieraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
