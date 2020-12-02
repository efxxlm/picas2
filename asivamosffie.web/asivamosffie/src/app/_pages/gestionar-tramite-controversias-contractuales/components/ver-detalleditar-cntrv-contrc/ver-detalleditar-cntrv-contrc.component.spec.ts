import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerDetalleditarCntrvContrcComponent } from './ver-detalleditar-cntrv-contrc.component';

describe('VerDetalleditarCntrvContrcComponent', () => {
  let component: VerDetalleditarCntrvContrcComponent;
  let fixture: ComponentFixture<VerDetalleditarCntrvContrcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VerDetalleditarCntrvContrcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerDetalleditarCntrvContrcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
