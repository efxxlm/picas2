import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCargarReportRendRprComponent } from './dialog-cargar-report-rend-rpr.component';

describe('DialogCargarReportRendRprComponent', () => {
  let component: DialogCargarReportRendRprComponent;
  let fixture: ComponentFixture<DialogCargarReportRendRprComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogCargarReportRendRprComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCargarReportRendRprComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
