import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSolicitudesEnviadasComponent } from './tabla-solicitudes-enviadas.component';

describe('TablaSolicitudesEnviadasComponent', () => {
  let component: TablaSolicitudesEnviadasComponent;
  let fixture: ComponentFixture<TablaSolicitudesEnviadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSolicitudesEnviadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSolicitudesEnviadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
