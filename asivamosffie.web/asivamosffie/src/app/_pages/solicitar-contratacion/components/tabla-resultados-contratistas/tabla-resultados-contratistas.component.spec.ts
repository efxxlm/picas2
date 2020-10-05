import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaResultadosContratistasComponent } from './tabla-resultados-contratistas.component';

describe('TablaResultadosContratistasComponent', () => {
  let component: TablaResultadosContratistasComponent;
  let fixture: ComponentFixture<TablaResultadosContratistasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaResultadosContratistasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaResultadosContratistasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
