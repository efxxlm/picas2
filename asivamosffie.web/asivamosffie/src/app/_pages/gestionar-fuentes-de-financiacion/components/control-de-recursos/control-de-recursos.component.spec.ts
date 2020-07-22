import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlDeRecursosComponent } from './control-de-recursos.component';

describe('ControlDeRecursosComponent', () => {
  let component: ControlDeRecursosComponent;
  let fixture: ComponentFixture<ControlDeRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlDeRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlDeRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
