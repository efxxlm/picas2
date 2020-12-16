import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratosObraVrtcComponent } from './tabla-contratos-obra-vrtc.component';

describe('TablaContratosObraVrtcComponent', () => {
  let component: TablaContratosObraVrtcComponent;
  let fixture: ComponentFixture<TablaContratosObraVrtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratosObraVrtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratosObraVrtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
