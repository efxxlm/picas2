import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleOgGbftrecComponent } from './detalle-og-gbftrec.component';

describe('DetalleOgGbftrecComponent', () => {
  let component: DetalleOgGbftrecComponent;
  let fixture: ComponentFixture<DetalleOgGbftrecComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleOgGbftrecComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleOgGbftrecComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
