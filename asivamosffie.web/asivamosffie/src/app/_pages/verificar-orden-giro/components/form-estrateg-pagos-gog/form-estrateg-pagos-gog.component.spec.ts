import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormEstrategPagosGogComponent } from './form-estrateg-pagos-gog.component';

describe('FormEstrategPagosGogComponent', () => {
  let component: FormEstrategPagosGogComponent;
  let fixture: ComponentFixture<FormEstrategPagosGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormEstrategPagosGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormEstrategPagosGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
