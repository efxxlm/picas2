import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarExpensasComponent } from './ver-detalle-editar-expensas.component';

describe('VerDetalleEditarExpensasComponent', () => {
  let component: VerDetalleEditarExpensasComponent;
  let fixture: ComponentFixture<VerDetalleEditarExpensasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarExpensasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarExpensasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
