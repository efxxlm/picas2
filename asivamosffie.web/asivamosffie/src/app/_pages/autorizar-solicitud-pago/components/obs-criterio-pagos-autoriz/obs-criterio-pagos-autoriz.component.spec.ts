import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsCriterioPagosAutorizComponent } from './obs-criterio-pagos-autoriz.component';

describe('ObsCriterioPagosAutorizComponent', () => {
  let component: ObsCriterioPagosAutorizComponent;
  let fixture: ComponentFixture<ObsCriterioPagosAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsCriterioPagosAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsCriterioPagosAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
