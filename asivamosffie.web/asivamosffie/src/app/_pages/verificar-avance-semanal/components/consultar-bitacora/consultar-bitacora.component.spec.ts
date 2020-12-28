import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultarBitacoraComponent } from './consultar-bitacora.component';

describe('ConsultarBitacoraComponent', () => {
  let component: ConsultarBitacoraComponent;
  let fixture: ComponentFixture<ConsultarBitacoraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConsultarBitacoraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConsultarBitacoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
