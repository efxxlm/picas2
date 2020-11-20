import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaActuacionReclamacionComponent } from './registrar-nueva-actuacion-reclamacion.component';

describe('RegistrarNuevaActuacionReclamacionComponent', () => {
  let component: RegistrarNuevaActuacionReclamacionComponent;
  let fixture: ComponentFixture<RegistrarNuevaActuacionReclamacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaActuacionReclamacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaActuacionReclamacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
