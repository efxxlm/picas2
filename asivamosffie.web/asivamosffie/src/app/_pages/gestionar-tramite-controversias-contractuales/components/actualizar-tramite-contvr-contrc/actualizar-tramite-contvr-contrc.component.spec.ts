import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarTramiteContvrContrcComponent } from './actualizar-tramite-contvr-contrc.component';

describe('ActualizarTramiteContvrContrcComponent', () => {
  let component: ActualizarTramiteContvrContrcComponent;
  let fixture: ComponentFixture<ActualizarTramiteContvrContrcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarTramiteContvrContrcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarTramiteContvrContrcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
