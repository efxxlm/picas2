import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarTramiteControvrContractComponent } from './gestionar-tramite-controvr-contract.component';

describe('GestionarTramiteControvrContractComponent', () => {
  let component: GestionarTramiteControvrContractComponent;
  let fixture: ComponentFixture<GestionarTramiteControvrContractComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarTramiteControvrContractComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarTramiteControvrContractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
