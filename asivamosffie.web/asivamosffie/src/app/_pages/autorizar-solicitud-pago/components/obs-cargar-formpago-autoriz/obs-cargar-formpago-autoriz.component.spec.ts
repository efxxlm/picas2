import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsCargarFormpagoAutorizComponent } from './obs-cargar-formpago-autoriz.component';

describe('ObsCargarFormpagoAutorizComponent', () => {
  let component: ObsCargarFormpagoAutorizComponent;
  let fixture: ComponentFixture<ObsCargarFormpagoAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsCargarFormpagoAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsCargarFormpagoAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
