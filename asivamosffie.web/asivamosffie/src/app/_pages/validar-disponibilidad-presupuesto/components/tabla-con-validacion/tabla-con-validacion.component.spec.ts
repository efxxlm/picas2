import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaConValidacionComponent } from './tabla-con-validacion.component';

describe('TablaConValidacionComponent', () => {
  let component: TablaConValidacionComponent;
  let fixture: ComponentFixture<TablaConValidacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaConValidacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaConValidacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
