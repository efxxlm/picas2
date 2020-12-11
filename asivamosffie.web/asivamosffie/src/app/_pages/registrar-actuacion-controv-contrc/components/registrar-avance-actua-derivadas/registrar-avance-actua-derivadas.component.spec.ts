import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarAvanceActuaDerivadasComponent } from './registrar-avance-actua-derivadas.component';

describe('RegistrarAvanceActuaDerivadasComponent', () => {
  let component: RegistrarAvanceActuaDerivadasComponent;
  let fixture: ComponentFixture<RegistrarAvanceActuaDerivadasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarAvanceActuaDerivadasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarAvanceActuaDerivadasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
