import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleInterventoriaComponent } from './ver-detalle-interventoria.component';

describe('VerDetalleInterventoriaComponent', () => {
  let component: VerDetalleInterventoriaComponent;
  let fixture: ComponentFixture<VerDetalleInterventoriaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleInterventoriaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleInterventoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
