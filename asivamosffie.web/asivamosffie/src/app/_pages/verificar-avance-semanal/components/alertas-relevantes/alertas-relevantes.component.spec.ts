import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AlertasRelevantesComponent } from './alertas-relevantes.component';

describe('AlertasRelevantesComponent', () => {
  let component: AlertasRelevantesComponent;
  let fixture: ComponentFixture<AlertasRelevantesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AlertasRelevantesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlertasRelevantesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
