import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogDevolverSolPagoGogComponent } from './dialog-devolver-sol-pago-gog.component';

describe('DialogDevolverSolPagoGogComponent', () => {
  let component: DialogDevolverSolPagoGogComponent;
  let fixture: ComponentFixture<DialogDevolverSolPagoGogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogDevolverSolPagoGogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogDevolverSolPagoGogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
