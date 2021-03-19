import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarTramiteLiqContractualComponent } from './gestionar-tramite-liq-contractual.component';

describe('GestionarTramiteLiqContractualComponent', () => {
  let component: GestionarTramiteLiqContractualComponent;
  let fixture: ComponentFixture<GestionarTramiteLiqContractualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarTramiteLiqContractualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarTramiteLiqContractualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
