import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleValidListchqAutorizComponent } from './detalle-valid-listchq-autoriz.component';

describe('DetalleValidListchqAutorizComponent', () => {
  let component: DetalleValidListchqAutorizComponent;
  let fixture: ComponentFixture<DetalleValidListchqAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleValidListchqAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleValidListchqAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
