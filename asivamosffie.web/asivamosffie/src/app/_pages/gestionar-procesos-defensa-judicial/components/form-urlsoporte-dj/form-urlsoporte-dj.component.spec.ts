import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormUrlsoporteDjComponent } from './form-urlsoporte-dj.component';

describe('FormUrlsoporteDjComponent', () => {
  let component: FormUrlsoporteDjComponent;
  let fixture: ComponentFixture<FormUrlsoporteDjComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormUrlsoporteDjComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormUrlsoporteDjComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
