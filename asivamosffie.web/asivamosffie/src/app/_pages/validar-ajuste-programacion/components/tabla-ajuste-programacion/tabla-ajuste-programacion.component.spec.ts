import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAjusteProgramacionComponent } from './tabla-ajuste-programacion.component';

describe('TablaAjusteProgramacionComponent', () => {
  let component: TablaAjusteProgramacionComponent;
  let fixture: ComponentFixture<TablaAjusteProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAjusteProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAjusteProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
