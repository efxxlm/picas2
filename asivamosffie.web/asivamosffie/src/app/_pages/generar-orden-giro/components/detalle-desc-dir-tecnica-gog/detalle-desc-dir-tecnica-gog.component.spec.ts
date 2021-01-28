import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleDescDirTecnicaGogComponent } from './detalle-desc-dir-tecnica-gog.component';

describe('DetalleDescDirTecnicaGogComponent', () => {
  let component: DetalleDescDirTecnicaGogComponent;
  let fixture: ComponentFixture<DetalleDescDirTecnicaGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleDescDirTecnicaGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleDescDirTecnicaGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
