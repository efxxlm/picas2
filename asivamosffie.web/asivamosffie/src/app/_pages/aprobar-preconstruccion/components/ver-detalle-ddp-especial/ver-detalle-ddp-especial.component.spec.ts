import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleDdpEspecialComponent } from './ver-detalle-ddp-especial.component';

describe('VerDetalleDdpEspecialComponent', () => {
  let component: VerDetalleDdpEspecialComponent;
  let fixture: ComponentFixture<VerDetalleDdpEspecialComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleDdpEspecialComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleDdpEspecialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
