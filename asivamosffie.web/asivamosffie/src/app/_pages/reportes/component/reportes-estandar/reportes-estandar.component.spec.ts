import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportesEstandarComponent } from './reportes-estandar.component';

describe('ReportesEstandarComponent', () => {
  let component: ReportesEstandarComponent;
  let fixture: ComponentFixture<ReportesEstandarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportesEstandarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportesEstandarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
