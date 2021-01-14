import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionarRendimientosComponent } from './gestionar-rendimientos.component';

describe('GestionarRendimientosComponent', () => {
  let component: GestionarRendimientosComponent;
  let fixture: ComponentFixture<GestionarRendimientosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GestionarRendimientosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GestionarRendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
