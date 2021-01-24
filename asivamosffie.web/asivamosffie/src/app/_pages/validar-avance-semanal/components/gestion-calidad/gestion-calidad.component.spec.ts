import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionCalidadComponent } from './gestion-calidad.component';

describe('GestionCalidadComponent', () => {
  let component: GestionCalidadComponent;
  let fixture: ComponentFixture<GestionCalidadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionCalidadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionCalidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
