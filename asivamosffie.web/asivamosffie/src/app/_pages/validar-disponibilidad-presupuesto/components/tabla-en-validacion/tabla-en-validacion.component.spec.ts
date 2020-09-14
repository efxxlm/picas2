import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEnValidacionComponent } from './tabla-en-validacion.component';

describe('TablaEnValidacionComponent', () => {
  let component: TablaEnValidacionComponent;
  let fixture: ComponentFixture<TablaEnValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEnValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEnValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
