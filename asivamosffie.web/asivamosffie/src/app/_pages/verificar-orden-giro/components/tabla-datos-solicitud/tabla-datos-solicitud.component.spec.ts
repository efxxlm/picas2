import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDatosSolicitudComponent } from './tabla-datos-solicitud.component';

describe('TablaDatosSolicitudComponent', () => {
  let component: TablaDatosSolicitudComponent;
  let fixture: ComponentFixture<TablaDatosSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDatosSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDatosSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
