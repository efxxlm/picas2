import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservacionesBalanceComponent } from './observaciones-balance.component';

describe('ObservacionesBalanceComponent', () => {
  let component: ObservacionesBalanceComponent;
  let fixture: ComponentFixture<ObservacionesBalanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservacionesBalanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservacionesBalanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
