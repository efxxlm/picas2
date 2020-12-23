import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInversionRecursosComponent } from './tabla-inversion-recursos.component';

describe('TablaInversionRecursosComponent', () => {
  let component: TablaInversionRecursosComponent;
  let fixture: ComponentFixture<TablaInversionRecursosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInversionRecursosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInversionRecursosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
