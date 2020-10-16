import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComiteFiduciarioComponent } from './comite-fiduciario.component';

describe('ComiteFiduciarioComponent', () => {
  let component: ComiteFiduciarioComponent;
  let fixture: ComponentFixture<ComiteFiduciarioComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComiteFiduciarioComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComiteFiduciarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
