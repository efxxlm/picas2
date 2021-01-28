import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SolicitudNovedadComponent } from './solicitud-novedad.component';

describe('SolicitudNovedadComponent', () => {
  let component: SolicitudNovedadComponent;
  let fixture: ComponentFixture<SolicitudNovedadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SolicitudNovedadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SolicitudNovedadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
