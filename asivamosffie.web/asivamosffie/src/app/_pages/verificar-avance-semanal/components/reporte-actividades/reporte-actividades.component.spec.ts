import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReporteActividadesComponent } from './reporte-actividades.component';

describe('ReporteActividadesComponent', () => {
  let component: ReporteActividadesComponent;
  let fixture: ComponentFixture<ReporteActividadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReporteActividadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReporteActividadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
