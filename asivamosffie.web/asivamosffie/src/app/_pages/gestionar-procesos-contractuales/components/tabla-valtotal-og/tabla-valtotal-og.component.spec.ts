import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaValtotalOgComponent } from './tabla-valtotal-og.component';

describe('TablaValtotalOgComponent', () => {
  let component: TablaValtotalOgComponent;
  let fixture: ComponentFixture<TablaValtotalOgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaValtotalOgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaValtotalOgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
