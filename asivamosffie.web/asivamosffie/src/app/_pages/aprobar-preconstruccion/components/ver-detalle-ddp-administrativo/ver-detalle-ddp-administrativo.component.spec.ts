import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleDdpAdministrativoComponent } from './ver-detalle-ddp-administrativo.component';

describe('VerDetalleDdpAdministrativoComponent', () => {
  let component: VerDetalleDdpAdministrativoComponent;
  let fixture: ComponentFixture<VerDetalleDdpAdministrativoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleDdpAdministrativoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleDdpAdministrativoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
