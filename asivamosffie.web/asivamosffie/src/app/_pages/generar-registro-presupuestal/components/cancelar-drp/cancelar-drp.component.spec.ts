import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CancelarDrpComponent } from './cancelar-drp.component';

describe('CancelarDrpComponent', () => {
  let component: CancelarDrpComponent;
  let fixture: ComponentFixture<CancelarDrpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CancelarDrpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CancelarDrpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
