import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevaSolicitudEspecialComponent } from './nueva-solicitud-especial.component';

describe('NuevaSolicitudEspecialComponent', () => {
  let component: NuevaSolicitudEspecialComponent;
  let fixture: ComponentFixture<NuevaSolicitudEspecialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NuevaSolicitudEspecialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NuevaSolicitudEspecialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
