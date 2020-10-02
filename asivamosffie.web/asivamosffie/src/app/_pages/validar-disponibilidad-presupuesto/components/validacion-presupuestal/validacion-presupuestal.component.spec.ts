import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidacionPresupuestalComponent } from './validacion-presupuestal.component';

describe('ValidacionPresupuestalComponent', () => {
  let component: ValidacionPresupuestalComponent;
  let fixture: ComponentFixture<ValidacionPresupuestalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidacionPresupuestalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidacionPresupuestalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
