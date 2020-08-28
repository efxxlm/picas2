import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFormSolicitudMultipleComponent } from './tabla-form-solicitud-multiple.component';

describe('TablaFormSolicitudMultipleComponent', () => {
  let component: TablaFormSolicitudMultipleComponent;
  let fixture: ComponentFixture<TablaFormSolicitudMultipleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFormSolicitudMultipleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFormSolicitudMultipleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
