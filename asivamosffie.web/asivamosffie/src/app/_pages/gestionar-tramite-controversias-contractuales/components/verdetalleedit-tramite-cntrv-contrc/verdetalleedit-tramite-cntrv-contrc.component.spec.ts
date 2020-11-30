import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleeditTramiteCntrvContrcComponent } from './verdetalleedit-tramite-cntrv-contrc.component';

describe('VerdetalleeditTramiteCntrvContrcComponent', () => {
  let component: VerdetalleeditTramiteCntrvContrcComponent;
  let fixture: ComponentFixture<VerdetalleeditTramiteCntrvContrcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleeditTramiteCntrvContrcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleeditTramiteCntrvContrcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
