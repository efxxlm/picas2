import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSolicitudesDevueltasComponent } from './tabla-solicitudes-devueltas.component';

describe('TablaSolicitudesDevueltasComponent', () => {
  let component: TablaSolicitudesDevueltasComponent;
  let fixture: ComponentFixture<TablaSolicitudesDevueltasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSolicitudesDevueltasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSolicitudesDevueltasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
