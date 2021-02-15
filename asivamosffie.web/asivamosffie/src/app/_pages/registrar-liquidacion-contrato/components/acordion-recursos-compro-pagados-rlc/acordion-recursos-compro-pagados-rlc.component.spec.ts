import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcordionRecursosComproPagadosRlcComponent } from './acordion-recursos-compro-pagados-rlc.component';

describe('AcordionRecursosComproPagadosRlcComponent', () => {
  let component: AcordionRecursosComproPagadosRlcComponent;
  let fixture: ComponentFixture<AcordionRecursosComproPagadosRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcordionRecursosComproPagadosRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcordionRecursosComproPagadosRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
