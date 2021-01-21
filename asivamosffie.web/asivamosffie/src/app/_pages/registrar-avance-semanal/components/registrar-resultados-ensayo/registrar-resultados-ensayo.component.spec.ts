import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarResultadosEnsayoComponent } from './registrar-resultados-ensayo.component';

describe('RegistrarResultadosEnsayoComponent', () => {
  let component: RegistrarResultadosEnsayoComponent;
  let fixture: ComponentFixture<RegistrarResultadosEnsayoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarResultadosEnsayoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarResultadosEnsayoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
