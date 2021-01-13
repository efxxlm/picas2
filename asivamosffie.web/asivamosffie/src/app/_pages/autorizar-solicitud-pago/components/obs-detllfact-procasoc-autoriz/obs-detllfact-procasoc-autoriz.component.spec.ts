import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsDetllfactProcasocAutorizComponent } from './obs-detllfact-procasoc-autoriz.component';

describe('ObsDetllfactProcasocAutorizComponent', () => {
  let component: ObsDetllfactProcasocAutorizComponent;
  let fixture: ComponentFixture<ObsDetllfactProcasocAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsDetllfactProcasocAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsDetllfactProcasocAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
