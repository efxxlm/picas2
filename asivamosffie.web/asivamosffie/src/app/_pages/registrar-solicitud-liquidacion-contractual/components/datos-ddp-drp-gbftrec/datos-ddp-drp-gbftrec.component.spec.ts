import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatosDdpDrpGbftrecComponent } from './datos-ddp-drp-gbftrec.component';

describe('DatosDdpDrpGbftrecComponent', () => {
  let component: DatosDdpDrpGbftrecComponent;
  let fixture: ComponentFixture<DatosDdpDrpGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatosDdpDrpGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatosDdpDrpGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
