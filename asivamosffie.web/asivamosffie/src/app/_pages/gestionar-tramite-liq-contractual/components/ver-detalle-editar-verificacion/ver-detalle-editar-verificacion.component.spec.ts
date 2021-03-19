import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleEditarVerificacionComponent } from './ver-detalle-editar-verificacion.component';

describe('VerDetalleEditarVerificacionComponent', () => {
  let component: VerDetalleEditarVerificacionComponent;
  let fixture: ComponentFixture<VerDetalleEditarVerificacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleEditarVerificacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleEditarVerificacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
