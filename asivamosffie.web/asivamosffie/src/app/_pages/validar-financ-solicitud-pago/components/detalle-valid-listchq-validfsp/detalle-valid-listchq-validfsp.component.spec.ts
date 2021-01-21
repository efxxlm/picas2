import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleValidListchqValidfspComponent } from './detalle-valid-listchq-validfsp.component';

describe('DetalleValidListchqValidfspComponent', () => {
  let component: DetalleValidListchqValidfspComponent;
  let fixture: ComponentFixture<DetalleValidListchqValidfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleValidListchqValidfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleValidListchqValidfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
