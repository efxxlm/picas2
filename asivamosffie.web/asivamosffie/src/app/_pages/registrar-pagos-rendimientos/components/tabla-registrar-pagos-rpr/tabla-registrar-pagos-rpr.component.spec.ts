import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistrarPagosRprComponent } from './tabla-registrar-pagos-rpr.component';

describe('TablaRegistrarPagosRprComponent', () => {
  let component: TablaRegistrarPagosRprComponent;
  let fixture: ComponentFixture<TablaRegistrarPagosRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistrarPagosRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistrarPagosRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
