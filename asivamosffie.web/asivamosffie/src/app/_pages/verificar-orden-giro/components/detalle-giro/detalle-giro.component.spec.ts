import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleGiroComponent } from './detalle-giro.component';

describe('DetalleGiroComponent', () => {
  let component: DetalleGiroComponent;
  let fixture: ComponentFixture<DetalleGiroComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleGiroComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleGiroComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
