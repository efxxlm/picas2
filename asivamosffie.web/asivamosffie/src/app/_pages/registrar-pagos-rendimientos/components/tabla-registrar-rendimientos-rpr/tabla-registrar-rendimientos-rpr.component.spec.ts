import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistrarRendimientosRprComponent } from './tabla-registrar-rendimientos-rpr.component';

describe('TablaRegistrarRendimientosRprComponent', () => {
  let component: TablaRegistrarRendimientosRprComponent;
  let fixture: ComponentFixture<TablaRegistrarRendimientosRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarRendimientosRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarRendimientosRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
