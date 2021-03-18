import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FichaContratosProyectosComponent } from './ficha-contratos-proyectos.component';

describe('FichaContratosProyectosComponent', () => {
  let component: FichaContratosProyectosComponent;
  let fixture: ComponentFixture<FichaContratosProyectosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FichaContratosProyectosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FichaContratosProyectosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
