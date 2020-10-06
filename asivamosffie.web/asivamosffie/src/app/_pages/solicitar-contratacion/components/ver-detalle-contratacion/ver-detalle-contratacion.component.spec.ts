import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleContratacionComponent } from './ver-detalle-contratacion.component';

describe('VerDetalleContratacionComponent', () => {
  let component: VerDetalleContratacionComponent;
  let fixture: ComponentFixture<VerDetalleContratacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleContratacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleContratacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
