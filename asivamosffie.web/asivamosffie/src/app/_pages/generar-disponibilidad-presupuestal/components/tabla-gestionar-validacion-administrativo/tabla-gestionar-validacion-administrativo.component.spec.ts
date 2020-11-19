import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGestionarValidacionAdministrativoComponent } from './tabla-gestionar-validacion-administrativo.component';

describe('TablaGestionarValidacionAdministrativoComponent', () => {
  let component: TablaGestionarValidacionAdministrativoComponent;
  let fixture: ComponentFixture<TablaGestionarValidacionAdministrativoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGestionarValidacionAdministrativoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGestionarValidacionAdministrativoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
