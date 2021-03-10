import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRemisionComponent } from './form-remision.component';

describe('FormRemisionComponent', () => {
  let component: FormRemisionComponent;
  let fixture: ComponentFixture<FormRemisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRemisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRemisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
