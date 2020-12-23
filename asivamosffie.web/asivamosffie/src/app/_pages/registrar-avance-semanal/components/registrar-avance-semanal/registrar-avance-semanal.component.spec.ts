import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrarAvanceSemanalComponent } from './registrar-avance-semanal.component';

describe('RegistrarAvanceSemanalComponent', () => {
  let component: RegistrarAvanceSemanalComponent;
  let fixture: ComponentFixture<RegistrarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegistrarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
