import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsCriterioPagosComponent } from './obs-criterio-pagos.component';

describe('ObsCriterioPagosComponent', () => {
  let component: ObsCriterioPagosComponent;
  let fixture: ComponentFixture<ObsCriterioPagosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsCriterioPagosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsCriterioPagosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
