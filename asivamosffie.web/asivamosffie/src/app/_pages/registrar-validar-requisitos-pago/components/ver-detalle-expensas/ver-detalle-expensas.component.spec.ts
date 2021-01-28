import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleExpensasComponent } from './ver-detalle-expensas.component';

describe('VerDetalleExpensasComponent', () => {
  let component: VerDetalleExpensasComponent;
  let fixture: ComponentFixture<VerDetalleExpensasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleExpensasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleExpensasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
