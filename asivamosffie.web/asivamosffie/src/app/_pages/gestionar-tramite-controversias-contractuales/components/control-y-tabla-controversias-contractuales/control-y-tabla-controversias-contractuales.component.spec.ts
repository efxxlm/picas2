import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaControversiasContractualesComponent } from './control-y-tabla-controversias-contractuales.component';

describe('ControlYTablaControversiasContractualesComponent', () => {
  let component: ControlYTablaControversiasContractualesComponent;
  let fixture: ComponentFixture<ControlYTablaControversiasContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaControversiasContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaControversiasContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
