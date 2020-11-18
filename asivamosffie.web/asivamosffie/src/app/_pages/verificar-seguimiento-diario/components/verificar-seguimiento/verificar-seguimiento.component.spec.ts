import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarSeguimientoComponent } from './verificar-seguimiento.component';

describe('VerificarSeguimientoComponent', () => {
  let component: VerificarSeguimientoComponent;
  let fixture: ComponentFixture<VerificarSeguimientoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarSeguimientoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarSeguimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
