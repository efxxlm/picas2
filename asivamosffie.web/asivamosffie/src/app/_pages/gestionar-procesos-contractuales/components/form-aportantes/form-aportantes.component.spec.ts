import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormAportantesComponent } from './form-aportantes.component';

describe('FormAportantesComponent', () => {
  let component: FormAportantesComponent;
  let fixture: ComponentFixture<FormAportantesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormAportantesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormAportantesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
