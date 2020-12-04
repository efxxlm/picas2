import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManejoAnticipoVerificarRequisitosComponent } from './manejo-anticipo-verificar-requisitos.component';

describe('ManejoAnticipoVerificarRequisitosComponent', () => {
  let component: ManejoAnticipoVerificarRequisitosComponent;
  let fixture: ComponentFixture<ManejoAnticipoVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManejoAnticipoVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManejoAnticipoVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
