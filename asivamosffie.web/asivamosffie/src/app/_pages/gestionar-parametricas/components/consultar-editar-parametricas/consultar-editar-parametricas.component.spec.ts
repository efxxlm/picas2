import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultarEditarParametricasComponent } from './consultar-editar-parametricas.component';

describe('ConsultarEditarParametricasComponent', () => {
  let component: ConsultarEditarParametricasComponent;
  let fixture: ComponentFixture<ConsultarEditarParametricasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsultarEditarParametricasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsultarEditarParametricasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
