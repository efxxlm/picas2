import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AprobarIncorporacionRendimientosComponent } from './aprobar-incorporacion-rendimientos.component';

describe('AprobarIncorporacionRendimientosComponent', () => {
  let component: AprobarIncorporacionRendimientosComponent;
  let fixture: ComponentFixture<AprobarIncorporacionRendimientosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AprobarIncorporacionRendimientosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AprobarIncorporacionRendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
