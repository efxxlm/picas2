import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaListaContratosComponent } from './tabla-lista-contratos.component';

describe('TablaListaContratosComponent', () => {
  let component: TablaListaContratosComponent;
  let fixture: ComponentFixture<TablaListaContratosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaListaContratosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaListaContratosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
