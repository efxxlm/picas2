import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarCumplimientoComponent } from './verificar-cumplimiento.component';

describe('VerificarCumplimientoComponent', () => {
  let component: VerificarCumplimientoComponent;
  let fixture: ComponentFixture<VerificarCumplimientoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarCumplimientoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarCumplimientoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
