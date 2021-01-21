import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleeditarMesaDeTrabajoComponent } from './ver-detalleeditar-mesa-de-trabajo.component';

describe('VerDetalleeditarMesaDeTrabajoComponent', () => {
  let component: VerDetalleeditarMesaDeTrabajoComponent;
  let fixture: ComponentFixture<VerDetalleeditarMesaDeTrabajoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleeditarMesaDeTrabajoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleeditarMesaDeTrabajoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
