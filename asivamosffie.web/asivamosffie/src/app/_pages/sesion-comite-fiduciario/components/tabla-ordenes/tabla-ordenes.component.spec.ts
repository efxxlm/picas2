import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaOrdenesComponent } from './tabla-ordenes.component';

describe('TablaOrdenesComponent', () => {
  let component: TablaOrdenesComponent;
  let fixture: ComponentFixture<TablaOrdenesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaOrdenesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaOrdenesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
