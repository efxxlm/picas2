import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDrpGtlcComponent } from './tabla-drp-gtlc.component';

describe('TablaDrpGtlcComponent', () => {
  let component: TablaDrpGtlcComponent;
  let fixture: ComponentFixture<TablaDrpGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDrpGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDrpGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
