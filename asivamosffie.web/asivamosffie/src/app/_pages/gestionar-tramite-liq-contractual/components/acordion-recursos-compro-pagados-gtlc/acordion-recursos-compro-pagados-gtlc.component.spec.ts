import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcordionRecursosComproPagadosGtlcComponent } from './acordion-recursos-compro-pagados-gtlc.component';

describe('AcordionRecursosComproPagadosGtlcComponent', () => {
  let component: AcordionRecursosComproPagadosGtlcComponent;
  let fixture: ComponentFixture<AcordionRecursosComproPagadosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcordionRecursosComproPagadosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcordionRecursosComproPagadosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
