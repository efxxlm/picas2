import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarNuevaMesatrabajoActComponent } from './registrar-nueva-mesatrabajo-act.component';

describe('RegistrarNuevaMesatrabajoActComponent', () => {
  let component: RegistrarNuevaMesatrabajoActComponent;
  let fixture: ComponentFixture<RegistrarNuevaMesatrabajoActComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarNuevaMesatrabajoActComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarNuevaMesatrabajoActComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
