import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValidacionesContractualesComponent } from './tabla-validaciones-contractuales.component';

describe('TablaValidacionesContractualesComponent', () => {
  let component: TablaValidacionesContractualesComponent;
  let fixture: ComponentFixture<TablaValidacionesContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValidacionesContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValidacionesContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
