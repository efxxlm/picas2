import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RazonTipoActualizacionRapgComponent } from './razon-tipo-actualizacion-rapg.component';

describe('RazonTipoActualizacionRapgComponent', () => {
  let component: RazonTipoActualizacionRapgComponent;
  let fixture: ComponentFixture<RazonTipoActualizacionRapgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RazonTipoActualizacionRapgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RazonTipoActualizacionRapgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
