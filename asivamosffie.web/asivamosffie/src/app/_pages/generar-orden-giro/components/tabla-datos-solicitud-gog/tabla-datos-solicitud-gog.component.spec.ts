import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDatosSolicitudGogComponent } from './tabla-datos-solicitud-gog.component';

describe('TablaDatosSolicitudGogComponent', () => {
  let component: TablaDatosSolicitudGogComponent;
  let fixture: ComponentFixture<TablaDatosSolicitudGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDatosSolicitudGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDatosSolicitudGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
