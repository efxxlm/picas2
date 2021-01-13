import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarReportPagosRprComponent } from './dialog-cargar-report-pagos-rpr.component';

describe('DialogCargarReportPagosRprComponent', () => {
  let component: DialogCargarReportPagosRprComponent;
  let fixture: ComponentFixture<DialogCargarReportPagosRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarReportPagosRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarReportPagosRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
