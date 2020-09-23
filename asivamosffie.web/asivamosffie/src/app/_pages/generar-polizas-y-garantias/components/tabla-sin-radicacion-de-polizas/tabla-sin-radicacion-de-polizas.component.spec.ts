import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaSinRadicacionDePolizasComponent } from './tabla-sin-radicacion-de-polizas.component';

describe('TablaSinRadicacionDePolizasComponent', () => {
  let component: TablaSinRadicacionDePolizasComponent;
  let fixture: ComponentFixture<TablaSinRadicacionDePolizasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaSinRadicacionDePolizasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaSinRadicacionDePolizasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
