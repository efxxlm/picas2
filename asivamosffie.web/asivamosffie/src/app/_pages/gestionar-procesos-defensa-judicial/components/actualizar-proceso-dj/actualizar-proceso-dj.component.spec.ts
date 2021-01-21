import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActualizarProcesoDjComponent } from './actualizar-proceso-dj.component';

describe('ActualizarProcesoDjComponent', () => {
  let component: ActualizarProcesoDjComponent;
  let fixture: ComponentFixture<ActualizarProcesoDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActualizarProcesoDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActualizarProcesoDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
