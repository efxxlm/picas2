import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarAvanceSemanalComponent } from './verificar-avance-semanal.component';

describe('VerificarAvanceSemanalComponent', () => {
  let component: VerificarAvanceSemanalComponent;
  let fixture: ComponentFixture<VerificarAvanceSemanalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarAvanceSemanalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarAvanceSemanalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
