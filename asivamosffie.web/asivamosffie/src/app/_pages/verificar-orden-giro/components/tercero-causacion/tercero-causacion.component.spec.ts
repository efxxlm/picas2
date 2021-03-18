import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TerceroCausacionComponent } from './tercero-causacion.component';

describe('TerceroCausacionComponent', () => {
  let component: TerceroCausacionComponent;
  let fixture: ComponentFixture<TerceroCausacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TerceroCausacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TerceroCausacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
