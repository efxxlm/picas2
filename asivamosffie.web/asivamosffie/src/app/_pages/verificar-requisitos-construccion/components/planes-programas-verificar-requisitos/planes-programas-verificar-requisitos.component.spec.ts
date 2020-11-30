import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanesProgramasVerificarRequisitosComponent } from './planes-programas-verificar-requisitos.component';

describe('PlanesProgramasVerificarRequisitosComponent', () => {
  let component: PlanesProgramasVerificarRequisitosComponent;
  let fixture: ComponentFixture<PlanesProgramasVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlanesProgramasVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanesProgramasVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
