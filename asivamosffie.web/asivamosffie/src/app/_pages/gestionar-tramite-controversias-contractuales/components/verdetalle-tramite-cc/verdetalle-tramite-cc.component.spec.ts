import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleTramiteCcComponent } from './verdetalle-tramite-cc.component';

describe('VerdetalleTramiteCcComponent', () => {
  let component: VerdetalleTramiteCcComponent;
  let fixture: ComponentFixture<VerdetalleTramiteCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleTramiteCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleTramiteCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
