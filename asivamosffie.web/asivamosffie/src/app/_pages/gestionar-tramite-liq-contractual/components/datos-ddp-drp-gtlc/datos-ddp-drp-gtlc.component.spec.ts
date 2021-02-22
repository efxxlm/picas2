import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosDdpDrpGtlcComponent } from './datos-ddp-drp-gtlc.component';

describe('DatosDdpDrpGtlcComponent', () => {
  let component: DatosDdpDrpGtlcComponent;
  let fixture: ComponentFixture<DatosDdpDrpGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosDdpDrpGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosDdpDrpGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
