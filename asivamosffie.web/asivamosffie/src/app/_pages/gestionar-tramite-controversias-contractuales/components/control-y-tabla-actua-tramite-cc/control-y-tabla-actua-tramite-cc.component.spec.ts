import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaActuaTramiteCcComponent } from './control-y-tabla-actua-tramite-cc.component';

describe('ControlYTablaActuaTramiteCcComponent', () => {
  let component: ControlYTablaActuaTramiteCcComponent;
  let fixture: ComponentFixture<ControlYTablaActuaTramiteCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaActuaTramiteCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaActuaTramiteCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
