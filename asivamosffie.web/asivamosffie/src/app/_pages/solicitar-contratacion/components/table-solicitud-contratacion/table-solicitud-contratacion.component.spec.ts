import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableSolicitudContratacionComponent } from './table-solicitud-contratacion.component';

describe('TableSolicitudContratacionComponent', () => {
  let component: TableSolicitudContratacionComponent;
  let fixture: ComponentFixture<TableSolicitudContratacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableSolicitudContratacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableSolicitudContratacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
