import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRecursosCompartidosComponent } from './tabla-recursos-compartidos.component';

describe('TablaRecursosCompartidosComponent', () => {
  let component: TablaRecursosCompartidosComponent;
  let fixture: ComponentFixture<TablaRecursosCompartidosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRecursosCompartidosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRecursosCompartidosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
