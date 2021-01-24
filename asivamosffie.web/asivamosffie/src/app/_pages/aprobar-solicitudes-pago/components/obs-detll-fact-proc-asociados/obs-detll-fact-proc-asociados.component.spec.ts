import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDetllFactProcAsociadosComponent } from './obs-detll-fact-proc-asociados.component';

describe('ObsDetllFactProcAsociadosComponent', () => {
  let component: ObsDetllFactProcAsociadosComponent;
  let fixture: ComponentFixture<ObsDetllFactProcAsociadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDetllFactProcAsociadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDetllFactProcAsociadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
