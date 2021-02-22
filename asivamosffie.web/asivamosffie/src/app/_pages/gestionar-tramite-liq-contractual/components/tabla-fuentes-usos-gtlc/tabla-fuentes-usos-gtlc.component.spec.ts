import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFuentesUsosGtlcComponent } from './tabla-fuentes-usos-gtlc.component';

describe('TablaFuentesUsosGtlcComponent', () => {
  let component: TablaFuentesUsosGtlcComponent;
  let fixture: ComponentFixture<TablaFuentesUsosGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFuentesUsosGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFuentesUsosGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
