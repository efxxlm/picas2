import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSinRegistroContratoComponent } from './tabla-sin-registro-contrato.component';

describe('TablaSinRegistroContratoComponent', () => {
  let component: TablaSinRegistroContratoComponent;
  let fixture: ComponentFixture<TablaSinRegistroContratoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSinRegistroContratoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSinRegistroContratoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
