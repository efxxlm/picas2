import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaldosYRendimientosComponent } from './saldos-y-rendimientos.component';

describe('SaldosYRendimientosComponent', () => {
  let component: SaldosYRendimientosComponent;
  let fixture: ComponentFixture<SaldosYRendimientosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaldosYRendimientosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaldosYRendimientosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
