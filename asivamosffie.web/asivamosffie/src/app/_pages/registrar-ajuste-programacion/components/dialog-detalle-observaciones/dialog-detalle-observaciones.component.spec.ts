import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDetalleObservacionesComponent } from './dialog-detalle-observaciones.component';

describe('DialogDetalleObservacionesComponent', () => {
  let component: DialogDetalleObservacionesComponent;
  let fixture: ComponentFixture<DialogDetalleObservacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogDetalleObservacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDetalleObservacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
