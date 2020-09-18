import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConValidacionPresupuestalComponent } from './con-validacion-presupuestal.component';

describe('ConValidacionPresupuestalComponent', () => {
  let component: ConValidacionPresupuestalComponent;
  let fixture: ComponentFixture<ConValidacionPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConValidacionPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConValidacionPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
