import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEjfinancieraGbftrecComponent } from './tabla-ejfinanciera-gbftrec.component';

describe('TablaEjfinancieraGbftrecComponent', () => {
  let component: TablaEjfinancieraGbftrecComponent;
  let fixture: ComponentFixture<TablaEjfinancieraGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEjfinancieraGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEjfinancieraGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
