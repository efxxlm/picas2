import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleDescrActCcComponent } from './detalle-descr-act-cc.component';

describe('DetalleDescrActCcComponent', () => {
  let component: DetalleDescrActCcComponent;
  let fixture: ComponentFixture<DetalleDescrActCcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleDescrActCcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleDescrActCcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
