import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSesionComiteTecnicoComponent } from './tabla-sesion-comite-tecnico.component';

describe('TablaSesionComiteTecnicoComponent', () => {
  let component: TablaSesionComiteTecnicoComponent;
  let fixture: ComponentFixture<TablaSesionComiteTecnicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSesionComiteTecnicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSesionComiteTecnicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
