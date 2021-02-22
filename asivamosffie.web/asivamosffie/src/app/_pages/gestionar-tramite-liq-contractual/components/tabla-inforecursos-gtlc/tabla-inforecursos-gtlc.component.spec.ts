import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaInforecursosGtlcComponent } from './tabla-inforecursos-gtlc.component';

describe('TablaInforecursosGtlcComponent', () => {
  let component: TablaInforecursosGtlcComponent;
  let fixture: ComponentFixture<TablaInforecursosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaInforecursosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaInforecursosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
