import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContratosModificacionesContractualesComponent } from './contratos-modificaciones-contractuales.component';

describe('ContratosModificacionesContractualesComponent', () => {
  let component: ContratosModificacionesContractualesComponent;
  let fixture: ComponentFixture<ContratosModificacionesContractualesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContratosModificacionesContractualesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContratosModificacionesContractualesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
