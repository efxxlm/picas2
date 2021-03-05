import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaListaChequeoComponent } from './tabla-lista-chequeo.component';

describe('TablaListaChequeoComponent', () => {
  let component: TablaListaChequeoComponent;
  let fixture: ComponentFixture<TablaListaChequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaListaChequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaListaChequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
