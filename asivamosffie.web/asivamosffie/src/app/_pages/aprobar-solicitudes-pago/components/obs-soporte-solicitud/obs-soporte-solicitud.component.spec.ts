import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsSoporteSolicitudComponent } from './obs-soporte-solicitud.component';

describe('ObsSoporteSolicitudComponent', () => {
  let component: ObsSoporteSolicitudComponent;
  let fixture: ComponentFixture<ObsSoporteSolicitudComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsSoporteSolicitudComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsSoporteSolicitudComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
