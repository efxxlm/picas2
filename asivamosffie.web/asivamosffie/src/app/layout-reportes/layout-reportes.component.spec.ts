import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutReportesComponent } from './layout-reportes.component';

describe('LayoutReportesComponent', () => {
  let component: LayoutReportesComponent;
  let fixture: ComponentFixture<LayoutReportesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutReportesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutReportesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
