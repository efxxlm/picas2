import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaRegistradosComponent } from './tabla-registrados.component';

describe('TablaRegistradosComponent', () => {
  let component: TablaRegistradosComponent;
  let fixture: ComponentFixture<TablaRegistradosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaRegistradosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaRegistradosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
