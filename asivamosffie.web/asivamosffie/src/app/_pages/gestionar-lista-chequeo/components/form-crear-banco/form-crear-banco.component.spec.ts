import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormCrearBancoComponent } from './form-crear-banco.component';

describe('FormCrearBancoComponent', () => {
  let component: FormCrearBancoComponent;
  let fixture: ComponentFixture<FormCrearBancoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormCrearBancoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormCrearBancoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
