import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosComproPagadosRlcComponent } from './recursos-compro-pagados-rlc.component';

describe('RecursosComproPagadosRlcComponent', () => {
  let component: RecursosComproPagadosRlcComponent;
  let fixture: ComponentFixture<RecursosComproPagadosRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosComproPagadosRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosComproPagadosRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
