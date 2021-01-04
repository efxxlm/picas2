import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarAjusteProgramacionComponent } from './registrar-ajuste-programacion.component';

describe('RegistrarAjusteProgramacionComponent', () => {
  let component: RegistrarAjusteProgramacionComponent;
  let fixture: ComponentFixture<RegistrarAjusteProgramacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarAjusteProgramacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarAjusteProgramacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
