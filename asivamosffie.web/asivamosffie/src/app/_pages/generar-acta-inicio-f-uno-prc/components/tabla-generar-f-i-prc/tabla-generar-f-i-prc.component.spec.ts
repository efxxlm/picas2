import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaGenerarFIPreconstruccionComponent } from './tabla-generar-f-i-prc.component';

describe('TablaGenerarFIPreconstruccionComponent', () => {
  let component: TablaGenerarFIPreconstruccionComponent;
  let fixture: ComponentFixture<TablaGenerarFIPreconstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaGenerarFIPreconstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaGenerarFIPreconstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
