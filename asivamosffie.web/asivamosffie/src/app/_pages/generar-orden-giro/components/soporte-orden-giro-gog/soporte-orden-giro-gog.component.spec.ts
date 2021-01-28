import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SoporteOrdenGiroGogComponent } from './soporte-orden-giro-gog.component';

describe('SoporteOrdenGiroGogComponent', () => {
  let component: SoporteOrdenGiroGogComponent;
  let fixture: ComponentFixture<SoporteOrdenGiroGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SoporteOrdenGiroGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SoporteOrdenGiroGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
