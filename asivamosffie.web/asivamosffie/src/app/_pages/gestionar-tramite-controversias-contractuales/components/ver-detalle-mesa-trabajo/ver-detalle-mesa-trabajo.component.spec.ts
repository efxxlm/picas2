import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleMesaTrabajoComponent } from './ver-detalle-mesa-trabajo.component';

describe('VerDetalleMesaTrabajoComponent', () => {
  let component: VerDetalleMesaTrabajoComponent;
  let fixture: ComponentFixture<VerDetalleMesaTrabajoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleMesaTrabajoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleMesaTrabajoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
