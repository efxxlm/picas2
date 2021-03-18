import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormRecorridoObraComponent } from './form-recorrido-obra.component';

describe('FormRecorridoObraComponent', () => {
  let component: FormRecorridoObraComponent;
  let fixture: ComponentFixture<FormRecorridoObraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormRecorridoObraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormRecorridoObraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
