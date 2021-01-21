import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsValidListachequeoComponent } from './obs-valid-listachequeo.component';

describe('ObsValidListachequeoComponent', () => {
  let component: ObsValidListachequeoComponent;
  let fixture: ComponentFixture<ObsValidListachequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsValidListachequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsValidListachequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
