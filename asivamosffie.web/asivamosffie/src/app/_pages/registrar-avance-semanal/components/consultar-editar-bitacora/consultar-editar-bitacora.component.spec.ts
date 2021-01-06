import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultarEditarBitacoraComponent } from './consultar-editar-bitacora.component';

describe('ConsultarEditarBitacoraComponent', () => {
  let component: ConsultarEditarBitacoraComponent;
  let fixture: ComponentFixture<ConsultarEditarBitacoraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsultarEditarBitacoraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsultarEditarBitacoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
