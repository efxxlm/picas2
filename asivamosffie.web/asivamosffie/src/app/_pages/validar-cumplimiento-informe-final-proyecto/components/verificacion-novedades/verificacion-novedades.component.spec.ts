import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificacionNovedadesComponent } from './verificacion-novedades.component';

describe('VerificacionNovedadesComponent', () => {
  let component: VerificacionNovedadesComponent;
  let fixture: ComponentFixture<VerificacionNovedadesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificacionNovedadesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificacionNovedadesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
