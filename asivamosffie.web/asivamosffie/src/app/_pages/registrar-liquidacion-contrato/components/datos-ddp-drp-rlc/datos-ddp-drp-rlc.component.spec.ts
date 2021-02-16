import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosDdpDrpRlcComponent } from './datos-ddp-drp-rlc.component';

describe('DatosDdpDrpRlcComponent', () => {
  let component: DatosDdpDrpRlcComponent;
  let fixture: ComponentFixture<DatosDdpDrpRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosDdpDrpRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosDdpDrpRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
