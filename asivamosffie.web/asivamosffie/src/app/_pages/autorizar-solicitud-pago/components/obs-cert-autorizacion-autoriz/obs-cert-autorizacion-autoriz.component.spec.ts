import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObsCertAutorizacionAutorizComponent } from './obs-cert-autorizacion-autoriz.component';

describe('ObsCertAutorizacionAutorizComponent', () => {
  let component: ObsCertAutorizacionAutorizComponent;
  let fixture: ComponentFixture<ObsCertAutorizacionAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObsCertAutorizacionAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObsCertAutorizacionAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
