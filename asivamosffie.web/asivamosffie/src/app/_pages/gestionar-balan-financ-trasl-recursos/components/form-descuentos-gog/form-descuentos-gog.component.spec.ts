import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDescuentosGogComponent } from './form-descuentos-gog.component';

describe('FormDescuentosGogComponent', () => {
  let component: FormDescuentosGogComponent;
  let fixture: ComponentFixture<FormDescuentosGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormDescuentosGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormDescuentosGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
