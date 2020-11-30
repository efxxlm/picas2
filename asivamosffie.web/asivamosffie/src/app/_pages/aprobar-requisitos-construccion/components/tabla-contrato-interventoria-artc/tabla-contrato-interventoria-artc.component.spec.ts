import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TablaContratoInterventoriaArtcComponent } from './tabla-contrato-interventoria-artc.component';

describe('TablaContratoInterventoriaArtcComponent', () => {
  let component: TablaContratoInterventoriaArtcComponent;
  let fixture: ComponentFixture<TablaContratoInterventoriaArtcComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TablaContratoInterventoriaArtcComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TablaContratoInterventoriaArtcComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
