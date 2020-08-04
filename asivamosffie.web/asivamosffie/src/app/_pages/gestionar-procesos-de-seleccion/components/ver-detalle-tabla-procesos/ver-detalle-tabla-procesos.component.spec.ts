import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleTablaProcesosComponent } from './ver-detalle-tabla-procesos.component';

describe('VerDetalleTablaProcesosComponent', () => {
  let component: VerDetalleTablaProcesosComponent;
  let fixture: ComponentFixture<VerDetalleTablaProcesosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleTablaProcesosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleTablaProcesosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
