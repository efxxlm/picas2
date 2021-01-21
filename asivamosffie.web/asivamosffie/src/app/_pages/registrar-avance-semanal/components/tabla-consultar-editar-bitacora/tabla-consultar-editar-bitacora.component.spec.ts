import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConsultarEditarBitacoraComponent } from './tabla-consultar-editar-bitacora.component';

describe('TablaConsultarEditarBitacoraComponent', () => {
  let component: TablaConsultarEditarBitacoraComponent;
  let fixture: ComponentFixture<TablaConsultarEditarBitacoraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConsultarEditarBitacoraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConsultarEditarBitacoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
