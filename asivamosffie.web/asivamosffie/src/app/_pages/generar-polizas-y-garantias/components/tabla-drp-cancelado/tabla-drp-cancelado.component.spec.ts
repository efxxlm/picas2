import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaDrpCanceladoComponent } from './tabla-drp-cancelado.component';

describe('TablaConAprobacionDePolizasComponent', () => {
  let component: TablaDrpCanceladoComponent;
  let fixture: ComponentFixture<TablaDrpCanceladoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaDrpCanceladoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaDrpCanceladoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
