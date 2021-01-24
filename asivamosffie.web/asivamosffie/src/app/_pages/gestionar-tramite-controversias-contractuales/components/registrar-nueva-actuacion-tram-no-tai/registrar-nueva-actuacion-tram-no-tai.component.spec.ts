import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaActuacionTramNoTaiComponent } from './registrar-nueva-actuacion-tram-no-tai.component';

describe('RegistrarNuevaActuacionTramNoTaiComponent', () => {
  let component: RegistrarNuevaActuacionTramNoTaiComponent;
  let fixture: ComponentFixture<RegistrarNuevaActuacionTramNoTaiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaActuacionTramNoTaiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaActuacionTramNoTaiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
