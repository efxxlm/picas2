import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtrosManejosComponent } from './otros-manejos.component';

describe('OtrosManejosComponent', () => {
  let component: OtrosManejosComponent;
  let fixture: ComponentFixture<OtrosManejosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtrosManejosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtrosManejosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
