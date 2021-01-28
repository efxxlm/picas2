import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsSoporteurlAutorizComponent } from './obs-soporteurl-autoriz.component';

describe('ObsSoporteurlAutorizComponent', () => {
  let component: ObsSoporteurlAutorizComponent;
  let fixture: ComponentFixture<ObsSoporteurlAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsSoporteurlAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsSoporteurlAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
