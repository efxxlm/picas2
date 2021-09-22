import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesOrdenGiroGogComponent } from './observaciones-orden-giro-gog.component';

describe('ObservacionesOrdenGiroGogComponent', () => {
  let component: ObservacionesOrdenGiroGogComponent;
  let fixture: ComponentFixture<ObservacionesOrdenGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesOrdenGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesOrdenGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
