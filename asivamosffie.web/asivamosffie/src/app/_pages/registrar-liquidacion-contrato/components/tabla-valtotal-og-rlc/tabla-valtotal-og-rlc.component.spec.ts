import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValtotalOgRlcComponent } from './tabla-valtotal-og-rlc.component';

describe('TablaValtotalOgRlcComponent', () => {
  let component: TablaValtotalOgRlcComponent;
  let fixture: ComponentFixture<TablaValtotalOgRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValtotalOgRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValtotalOgRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
