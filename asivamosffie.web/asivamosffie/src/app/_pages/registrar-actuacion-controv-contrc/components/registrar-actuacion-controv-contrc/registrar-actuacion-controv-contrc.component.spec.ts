import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarActuacionControvContrcComponent } from './registrar-actuacion-controv-contrc.component';

describe('RegistrarActuacionControvContrcComponent', () => {
  let component: RegistrarActuacionControvContrcComponent;
  let fixture: ComponentFixture<RegistrarActuacionControvContrcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarActuacionControvContrcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarActuacionControvContrcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
