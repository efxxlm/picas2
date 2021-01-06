import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerdetalleReclamacionActuacionCcComponent } from './verdetalle-reclamacion-actuacion-cc.component';

describe('VerdetalleReclamacionActuacionCcComponent', () => {
  let component: VerdetalleReclamacionActuacionCcComponent;
  let fixture: ComponentFixture<VerdetalleReclamacionActuacionCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerdetalleReclamacionActuacionCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerdetalleReclamacionActuacionCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
