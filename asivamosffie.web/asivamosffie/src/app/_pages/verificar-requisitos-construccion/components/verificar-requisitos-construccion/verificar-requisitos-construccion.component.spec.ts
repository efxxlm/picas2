import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarRequisitosConstruccionComponent } from './verificar-requisitos-construccion.component';

describe('VerificarRequisitosConstruccionComponent', () => {
  let component: VerificarRequisitosConstruccionComponent;
  let fixture: ComponentFixture<VerificarRequisitosConstruccionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarRequisitosConstruccionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarRequisitosConstruccionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
