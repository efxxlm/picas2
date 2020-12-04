import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistroProgramacionComponent } from './tabla-registro-programacion.component';

describe('TablaRegistroProgramacionComponent', () => {
  let component: TablaRegistroProgramacionComponent;
  let fixture: ComponentFixture<TablaRegistroProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistroProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistroProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
