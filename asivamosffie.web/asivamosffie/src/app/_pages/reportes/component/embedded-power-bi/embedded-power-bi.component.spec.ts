import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmbeddedPowerBiComponent } from './embedded-power-bi.component';

describe('EmbeddedPowerBiComponent', () => {
  let component: EmbeddedPowerBiComponent;
  let fixture: ComponentFixture<EmbeddedPowerBiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmbeddedPowerBiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmbeddedPowerBiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
