import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSolicitudNovedadContractualComponent } from './tabla-solicitud-novedad-contractual.component';

describe('TablaSolicitudNovedadContractualComponent', () => {
  let component: TablaSolicitudNovedadContractualComponent;
  let fixture: ComponentFixture<TablaSolicitudNovedadContractualComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSolicitudNovedadContractualComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSolicitudNovedadContractualComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
