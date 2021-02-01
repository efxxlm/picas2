import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewDetllFactProcAsociadosComponent } from './view-detll-fact-proc-asociados.component';

describe('ViewDetllFactProcAsociadosComponent', () => {
  let component: ViewDetllFactProcAsociadosComponent;
  let fixture: ComponentFixture<ViewDetllFactProcAsociadosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewDetllFactProcAsociadosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewDetllFactProcAsociadosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
