import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerificarInformeGtlcComponent } from './verificar-informe-gtlc.component';

describe('VerificarInformeGtlcComponent', () => {
  let component: VerificarInformeGtlcComponent;
  let fixture: ComponentFixture<VerificarInformeGtlcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerificarInformeGtlcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerificarInformeGtlcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
