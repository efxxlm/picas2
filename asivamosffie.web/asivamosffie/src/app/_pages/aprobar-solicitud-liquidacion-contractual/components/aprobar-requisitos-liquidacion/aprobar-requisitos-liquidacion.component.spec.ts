import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarRequisitosLiquidacionComponent } from './aprobar-requisitos-liquidacion.component';

describe('AprobarRequisitosLiquidacionComponent', () => {
  let component: AprobarRequisitosLiquidacionComponent;
  let fixture: ComponentFixture<AprobarRequisitosLiquidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarRequisitosLiquidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarRequisitosLiquidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
