import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidarInformeComponent } from './validar-informe.component';

describe('ValidarInformeComponent', () => {
  let component: ValidarInformeComponent;
  let fixture: ComponentFixture<ValidarInformeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidarInformeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidarInformeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
