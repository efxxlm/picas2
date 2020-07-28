import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaAcuerdosComponent } from './tabla-acuerdos.component';

describe('TablaAcuerdosComponent', () => {
  let component: TablaAcuerdosComponent;
  let fixture: ComponentFixture<TablaAcuerdosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaAcuerdosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaAcuerdosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
