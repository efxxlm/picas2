import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlYTablaReclamacionCcComponent } from './control-y-tabla-reclamacion-cc.component';

describe('ControlYTablaReclamacionCcComponent', () => {
  let component: ControlYTablaReclamacionCcComponent;
  let fixture: ComponentFixture<ControlYTablaReclamacionCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlYTablaReclamacionCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlYTablaReclamacionCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
