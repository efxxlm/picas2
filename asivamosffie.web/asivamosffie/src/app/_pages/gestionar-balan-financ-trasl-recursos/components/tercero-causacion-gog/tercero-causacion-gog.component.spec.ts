import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TerceroCausacionGogComponent } from './tercero-causacion-gog.component';

describe('TerceroCausacionGogComponent', () => {
  let component: TerceroCausacionGogComponent;
  let fixture: ComponentFixture<TerceroCausacionGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TerceroCausacionGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TerceroCausacionGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
