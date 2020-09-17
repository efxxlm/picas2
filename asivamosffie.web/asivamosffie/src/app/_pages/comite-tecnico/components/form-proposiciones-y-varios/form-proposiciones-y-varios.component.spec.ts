import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormProposicionesYVariosComponent } from './form-proposiciones-y-varios.component';

describe('FormProposicionesYVariosComponent', () => {
  let component: FormProposicionesYVariosComponent;
  let fixture: ComponentFixture<FormProposicionesYVariosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormProposicionesYVariosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormProposicionesYVariosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
