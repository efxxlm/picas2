import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInformeFinalProyectoComponent } from './tabla-informe-final-proyecto.component';

describe('TablaInformeFinalProyectoComponent', () => {
  let component: TablaInformeFinalProyectoComponent;
  let fixture: ComponentFixture<TablaInformeFinalProyectoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInformeFinalProyectoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInformeFinalProyectoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
