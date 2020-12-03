import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarRequisitosConstruccionComponent } from './aprobar-requisitos-construccion.component';

describe('AprobarRequisitosConstruccionComponent', () => {
  let component: AprobarRequisitosConstruccionComponent;
  let fixture: ComponentFixture<AprobarRequisitosConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarRequisitosConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarRequisitosConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
