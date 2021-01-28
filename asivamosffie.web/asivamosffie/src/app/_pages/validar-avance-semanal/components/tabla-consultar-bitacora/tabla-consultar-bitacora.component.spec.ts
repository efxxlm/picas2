import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConsultarBitacoraComponent } from './tabla-consultar-bitacora.component';

describe('TablaConsultarBitacoraComponent', () => {
  let component: TablaConsultarBitacoraComponent;
  let fixture: ComponentFixture<TablaConsultarBitacoraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConsultarBitacoraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConsultarBitacoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
