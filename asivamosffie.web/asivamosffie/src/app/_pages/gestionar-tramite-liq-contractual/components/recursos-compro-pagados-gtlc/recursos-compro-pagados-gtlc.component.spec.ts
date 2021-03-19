import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosComproPagadosGtlcComponent } from './recursos-compro-pagados-gtlc.component';

describe('RecursosComproPagadosGtlcComponent', () => {
  let component: RecursosComproPagadosGtlcComponent;
  let fixture: ComponentFixture<RecursosComproPagadosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosComproPagadosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosComproPagadosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
