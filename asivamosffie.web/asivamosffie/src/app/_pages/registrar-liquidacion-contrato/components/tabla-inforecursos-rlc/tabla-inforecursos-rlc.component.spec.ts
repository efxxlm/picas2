import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInforecursosRlcComponent } from './tabla-inforecursos-rlc.component';

describe('TablaInforecursosRlcComponent', () => {
  let component: TablaInforecursosRlcComponent;
  let fixture: ComponentFixture<TablaInforecursosRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInforecursosRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInforecursosRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
