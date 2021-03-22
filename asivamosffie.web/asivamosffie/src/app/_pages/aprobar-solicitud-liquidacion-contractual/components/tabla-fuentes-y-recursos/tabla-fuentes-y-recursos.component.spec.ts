import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFuentesYRecursosComponent } from './tabla-fuentes-y-recursos.component';

describe('TablaFuentesYRecursosComponent', () => {
  let component: TablaFuentesYRecursosComponent;
  let fixture: ComponentFixture<TablaFuentesYRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFuentesYRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFuentesYRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
