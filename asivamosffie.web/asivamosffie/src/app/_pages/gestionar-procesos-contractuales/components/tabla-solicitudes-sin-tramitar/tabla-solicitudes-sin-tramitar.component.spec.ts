import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSolicitudesSinTramitarComponent } from './tabla-solicitudes-sin-tramitar.component';

describe('TablaSolicitudesSinTramitarComponent', () => {
  let component: TablaSolicitudesSinTramitarComponent;
  let fixture: ComponentFixture<TablaSolicitudesSinTramitarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSolicitudesSinTramitarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSolicitudesSinTramitarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
