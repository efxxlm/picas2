import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsValidListchqeoAutorizComponent } from './obs-valid-listchqeo-autoriz.component';

describe('ObsValidListchqeoAutorizComponent', () => {
  let component: ObsValidListchqeoAutorizComponent;
  let fixture: ComponentFixture<ObsValidListchqeoAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsValidListchqeoAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsValidListchqeoAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
