import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableCaracteristicasEspecialesComponent } from './table-caracteristicas-especiales.component';

describe('TableCaracteristicasEspecialesComponent', () => {
  let component: TableCaracteristicasEspecialesComponent;
  let fixture: ComponentFixture<TableCaracteristicasEspecialesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableCaracteristicasEspecialesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableCaracteristicasEspecialesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
