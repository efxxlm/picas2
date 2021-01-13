import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleValidListchqVfspComponent } from './detalle-valid-listchq-vfsp.component';

describe('DetalleValidListchqVfspComponent', () => {
  let component: DetalleValidListchqVfspComponent;
  let fixture: ComponentFixture<DetalleValidListchqVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleValidListchqVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleValidListchqVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
