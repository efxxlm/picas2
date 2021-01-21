import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisualizarAvanceObraTiempoRealComponent } from './visualizar-avance-obra-tiempo-real.component';

describe('VisualizarAvanceObraTiempoRealComponent', () => {
  let component: VisualizarAvanceObraTiempoRealComponent;
  let fixture: ComponentFixture<VisualizarAvanceObraTiempoRealComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VisualizarAvanceObraTiempoRealComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisualizarAvanceObraTiempoRealComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
