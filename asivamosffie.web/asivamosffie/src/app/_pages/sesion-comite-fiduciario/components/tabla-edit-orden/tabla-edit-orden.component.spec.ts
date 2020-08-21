import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaEditOrdenComponent } from './tabla-edit-orden.component';

describe('TablaEditOrdenComponent', () => {
  let component: TablaEditOrdenComponent;
  let fixture: ComponentFixture<TablaEditOrdenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaEditOrdenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaEditOrdenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
