import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConAprobacionDePolizasComponent } from './tabla-con-aprobacion-de-polizas.component';

describe('TablaConAprobacionDePolizasComponent', () => {
  let component: TablaConAprobacionDePolizasComponent;
  let fixture: ComponentFixture<TablaConAprobacionDePolizasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConAprobacionDePolizasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConAprobacionDePolizasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
