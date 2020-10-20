import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarRegistroPresupuestalComponent } from './generar-registro-presupuestal.component';

describe('GenerarRegistroPresupuestalComponent', () => {
  let component: GenerarRegistroPresupuestalComponent;
  let fixture: ComponentFixture<GenerarRegistroPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerarRegistroPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerarRegistroPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
