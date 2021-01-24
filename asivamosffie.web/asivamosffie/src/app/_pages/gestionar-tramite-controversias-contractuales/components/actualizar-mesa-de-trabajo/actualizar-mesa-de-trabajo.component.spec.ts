import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarMesaDeTrabajoComponent } from './actualizar-mesa-de-trabajo.component';

describe('ActualizarMesaDeTrabajoComponent', () => {
  let component: ActualizarMesaDeTrabajoComponent;
  let fixture: ComponentFixture<ActualizarMesaDeTrabajoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarMesaDeTrabajoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarMesaDeTrabajoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
