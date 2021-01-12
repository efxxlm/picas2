import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HojasVidaVerificarRequisitosComponent } from './hojas-vida-verificar-requisitos.component';

describe('HojasVidaVerificarRequisitosComponent', () => {
  let component: HojasVidaVerificarRequisitosComponent;
  let fixture: ComponentFixture<HojasVidaVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HojasVidaVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HojasVidaVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
