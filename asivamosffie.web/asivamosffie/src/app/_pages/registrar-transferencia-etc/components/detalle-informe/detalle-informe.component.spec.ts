import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleInformeComponent } from './detalle-informe.component';

describe('DetalleInformeComponent', () => {
  let component: DetalleInformeComponent;
  let fixture: ComponentFixture<DetalleInformeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleInformeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleInformeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
