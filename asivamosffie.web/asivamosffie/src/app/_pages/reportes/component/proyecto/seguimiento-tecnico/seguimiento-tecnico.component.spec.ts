import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeguimientoTecnicoComponent } from './seguimiento-tecnico.component';

describe('SeguimientoTecnicoComponent', () => {
  let component: SeguimientoTecnicoComponent;
  let fixture: ComponentFixture<SeguimientoTecnicoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeguimientoTecnicoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeguimientoTecnicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
