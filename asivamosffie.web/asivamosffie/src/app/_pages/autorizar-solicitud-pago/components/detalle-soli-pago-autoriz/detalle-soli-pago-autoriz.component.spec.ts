import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleSoliPagoAutorizComponent } from './detalle-soli-pago-autoriz.component';

describe('DetalleSoliPagoAutorizComponent', () => {
  let component: DetalleSoliPagoAutorizComponent;
  let fixture: ComponentFixture<DetalleSoliPagoAutorizComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetalleSoliPagoAutorizComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleSoliPagoAutorizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
