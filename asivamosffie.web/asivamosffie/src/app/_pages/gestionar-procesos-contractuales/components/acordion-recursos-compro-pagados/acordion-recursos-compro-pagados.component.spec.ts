import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcordionRecursosComproPagadosComponent } from './acordion-recursos-compro-pagados.component';

describe('AcordionRecursosComproPagadosComponent', () => {
  let component: AcordionRecursosComproPagadosComponent;
  let fixture: ComponentFixture<AcordionRecursosComproPagadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcordionRecursosComproPagadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcordionRecursosComproPagadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
