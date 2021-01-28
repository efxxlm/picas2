import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarTramiteRaccComponent } from './actualizar-tramite-racc.component';

describe('ActualizarTramiteRaccComponent', () => {
  let component: ActualizarTramiteRaccComponent;
  let fixture: ComponentFixture<ActualizarTramiteRaccComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarTramiteRaccComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarTramiteRaccComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
