import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaNovedadesAprobadasComponent } from './tabla-novedades-aprobadas.component';

describe('TablaNovedadesAprobadasComponent', () => {
  let component: TablaNovedadesAprobadasComponent;
  let fixture: ComponentFixture<TablaNovedadesAprobadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaNovedadesAprobadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaNovedadesAprobadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
