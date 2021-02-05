import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDrpGbftrecComponent } from './tabla-drp-gbftrec.component';

describe('TablaDrpGbftrecComponent', () => {
  let component: TablaDrpGbftrecComponent;
  let fixture: ComponentFixture<TablaDrpGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDrpGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDrpGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
