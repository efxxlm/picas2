import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleTerceroCausacionGogComponent } from './detalle-tercero-causacion-gog.component';

describe('DetalleTerceroCausacionGogComponent', () => {
  let component: DetalleTerceroCausacionGogComponent;
  let fixture: ComponentFixture<DetalleTerceroCausacionGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleTerceroCausacionGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleTerceroCausacionGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
