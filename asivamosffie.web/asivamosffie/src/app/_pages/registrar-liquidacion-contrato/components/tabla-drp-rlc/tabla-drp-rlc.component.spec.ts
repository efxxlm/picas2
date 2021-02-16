import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDrpRlcComponent } from './tabla-drp-rlc.component';

describe('TablaDrpRlcComponent', () => {
  let component: TablaDrpRlcComponent;
  let fixture: ComponentFixture<TablaDrpRlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDrpRlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDrpRlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
