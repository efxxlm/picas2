import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormListaChequeoComponent } from './form-lista-chequeo.component';

describe('FormListaChequeoComponent', () => {
  let component: FormListaChequeoComponent;
  let fixture: ComponentFixture<FormListaChequeoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormListaChequeoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormListaChequeoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
