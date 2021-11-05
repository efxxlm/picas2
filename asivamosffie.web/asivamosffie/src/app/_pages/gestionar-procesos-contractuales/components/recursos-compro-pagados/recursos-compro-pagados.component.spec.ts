import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosComproPagadosComponent } from './recursos-compro-pagados.component';

describe('RecursosComproPagadosComponent', () => {
  let component: RecursosComproPagadosComponent;
  let fixture: ComponentFixture<RecursosComproPagadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosComproPagadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosComproPagadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
