import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InversionFljrecursosVerificarRequisitosComponent } from './inversion-fljrecursos-verificar-requisitos.component';

describe('InversionFljrecursosVerificarRequisitosComponent', () => {
  let component: InversionFljrecursosVerificarRequisitosComponent;
  let fixture: ComponentFixture<InversionFljrecursosVerificarRequisitosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InversionFljrecursosVerificarRequisitosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InversionFljrecursosVerificarRequisitosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
