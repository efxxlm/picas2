import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecursosComprometidosPagadosComponent } from './recursos-comprometidos-pagados.component';

describe('RecursosComprometidosPagadosComponent', () => {
  let component: RecursosComprometidosPagadosComponent;
  let fixture: ComponentFixture<RecursosComprometidosPagadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecursosComprometidosPagadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecursosComprometidosPagadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
