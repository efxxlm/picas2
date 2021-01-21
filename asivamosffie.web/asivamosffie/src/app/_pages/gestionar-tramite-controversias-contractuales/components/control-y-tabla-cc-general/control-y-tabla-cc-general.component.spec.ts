import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaCcGeneralComponent } from './control-y-tabla-cc-general.component';

describe('ControlYTablaCcGeneralComponent', () => {
  let component: ControlYTablaCcGeneralComponent;
  let fixture: ComponentFixture<ControlYTablaCcGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaCcGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaCcGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
