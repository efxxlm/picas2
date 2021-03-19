import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaFuentesUsosRlcComponent } from './tabla-fuentes-usos-rlc.component';

describe('TablaFuentesUsosRlcComponent', () => {
  let component: TablaFuentesUsosRlcComponent;
  let fixture: ComponentFixture<TablaFuentesUsosRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaFuentesUsosRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaFuentesUsosRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
