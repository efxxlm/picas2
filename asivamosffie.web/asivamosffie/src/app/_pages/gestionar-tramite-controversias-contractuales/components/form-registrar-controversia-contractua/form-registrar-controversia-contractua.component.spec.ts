import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRegistrarControversiaContractuaComponent } from './form-registrar-controversia-contractua.component';

describe('FormRegistrarControversiaContractuaComponent', () => {
  let component: FormRegistrarControversiaContractuaComponent;
  let fixture: ComponentFixture<FormRegistrarControversiaContractuaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRegistrarControversiaContractuaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRegistrarControversiaContractuaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
