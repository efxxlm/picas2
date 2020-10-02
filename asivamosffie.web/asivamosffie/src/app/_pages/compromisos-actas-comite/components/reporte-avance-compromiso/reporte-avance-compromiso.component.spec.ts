import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteAvanceCompromisoComponent } from './reporte-avance-compromiso.component';

describe('ReporteAvanceCompromisoComponent', () => {
  let component: ReporteAvanceCompromisoComponent;
  let fixture: ComponentFixture<ReporteAvanceCompromisoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReporteAvanceCompromisoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteAvanceCompromisoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
